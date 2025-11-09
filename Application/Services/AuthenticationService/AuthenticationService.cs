namespace UltimateDotNetSkeleton.Application.Services.AuthenticationService
{
	using System;
	using System.IdentityModel.Tokens.Jwt;
	using System.Net.Mail;
	using System.Security.Claims;
	using System.Security.Cryptography;
	using System.Text;

	using AutoMapper;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.IdentityModel.Tokens;
	using Serilog;
	using UltimateDotNetSkeleton.Application.ConfigurationModels;
	using UltimateDotNetSkeleton.Application.DTOs.Authentication;
	using UltimateDotNetSkeleton.Application.DTOs.Token;
	using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;
	using UltimateDotNetSkeleton.Application.Exceptions.Conflict;
	using UltimateDotNetSkeleton.Application.Exceptions.NotFound;
	using UltimateDotNetSkeleton.Domain.Enums;
	using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Domain.Repositories.Manager;
	using UltimateDotNetSkeleton.Infrastructure.Services.DateTimeHelper;
	using UltimateDotNetSkeleton.Infrastructure.Services.EmailSender;

	public class AuthenticationService : IAuthenticationService
	{
		private readonly IRepositoryManager _repository;

		private readonly IMapper _mapper;

		private readonly IEmailSender _emailSender;

		private readonly JwtConfiguration _jwtConfiguration;

		public AuthenticationService(IRepositoryManager repositoryManager, IMapper mapper, IEmailSender emailSender, IConfiguration configuration)
		{
			_repository = repositoryManager;
			_mapper = mapper;
			_emailSender = emailSender;
			_jwtConfiguration = new JwtConfiguration();
			configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
		}

		public async Task<TokenDto> LoginAsync(UserForAuthenticationDto userForAuthentication)
		{
			var user = await _repository.User.GetByEmailAsync(userForAuthentication.Email!)
				?? throw new UserNotFoundException($"Korisnik sa email adresom '{userForAuthentication.Email!}' nije pronađen.");

			if (user.RegistrationType != RegistrationType.Native)
				throw new InvalidRegistrationTypeBadRequestException();

			if (!VerifyHash(userForAuthentication.Password!, user.PasswordHash!, user.PasswordSalt!))
				throw new InvalidPasswordBadRequestException();

			var refreshToken = await GetOrCreateRefreshTokenAsync(user.Id!, userForAuthentication.DeviceId!);

			return new TokenDto
			{
				AccessToken = GenerateAccessToken(user),
				RefreshToken = refreshToken.Token!,
			};
		}

		public async Task<TokenDto> GoogleLoginAsync(GoogleUserForAuthenticationDto googleUserForAuthentication)
		{
			var user = await _repository.User.GetByEmailAsync(googleUserForAuthentication.Email!)
				?? throw new UserNotFoundException($"Korisnik sa email adresom '{googleUserForAuthentication.Email!}' nije pronađen.");

			if (user.RegistrationType != RegistrationType.Google)
				throw new InvalidRegistrationTypeBadRequestException("Google");

			if (string.IsNullOrEmpty(user.ThirdPartyId) || user.ThirdPartyId != googleUserForAuthentication.GoogleId)
				throw new InvalidPasswordBadRequestException("Pogrešan Google ID.");

			var refreshToken = await GetOrCreateRefreshTokenAsync(user.Id!, googleUserForAuthentication.DeviceId!);

			return new TokenDto
			{
				AccessToken = GenerateAccessToken(user),
				RefreshToken = refreshToken.Token!,
			};
		}

		public async Task<TokenDto> AppleLoginAsync(AppleUserForAuthenticationDto appleUserForAuthentication)
		{
			var user = await _repository.User.GetByEmailAsync(appleUserForAuthentication.Email!)
				?? throw new UserNotFoundException($"Korisnik sa email adresom '{appleUserForAuthentication.Email!}' nije pronađen.");

			if (user.RegistrationType != RegistrationType.Apple)
				throw new InvalidRegistrationTypeBadRequestException("Apple");

			if (string.IsNullOrEmpty(user.ThirdPartyId) || user.ThirdPartyId != appleUserForAuthentication.AppleId)
				throw new InvalidPasswordBadRequestException("Pogrešan Apple ID.");

			var refreshToken = await GetOrCreateRefreshTokenAsync(user.Id!, appleUserForAuthentication.DeviceId!);

			return new TokenDto
			{
				AccessToken = GenerateAccessToken(user),
				RefreshToken = refreshToken.Token!,
			};
		}

		public async Task RegisterAsync(UserForRegistrationDto userForRegistration)
		{
			await CheckUserExistence(userForRegistration.Email!, userForRegistration.PhoneNumber!);

			var user = _mapper.Map<User>(userForRegistration);

			(user.PasswordHash, user.PasswordSalt) = CreateHash(userForRegistration.Password!);

			_repository.User.CreateUser(user);

			await _repository.SaveAsync();
		}

		public async Task<TokenDto> GoogleRegisterAsync(GoogleUserForRegistrationDto googleUserForRegistration)
		{
			await CheckUserExistence(googleUserForRegistration.Email!, googleUserForRegistration.PhoneNumber!);

			var user = _mapper.Map<User>(googleUserForRegistration);

			_repository.User.CreateUser(user);
			await _repository.SaveAsync();

			return await GoogleLoginAsync(new GoogleUserForAuthenticationDto
			{
				Email = googleUserForRegistration.Email,
				DeviceId = googleUserForRegistration.DeviceId,
				GoogleId = googleUserForRegistration.GoogleId,
			});
		}

		public async Task<TokenDto> AppleRegisterAsync(AppleUserForRegistrationDto appleUserForRegistration)
		{
			await CheckUserExistence(appleUserForRegistration.Email!, appleUserForRegistration.PhoneNumber!);

			var user = _mapper.Map<User>(appleUserForRegistration);

			_repository.User.CreateUser(user);
			await _repository.SaveAsync();

			return await AppleLoginAsync(new AppleUserForAuthenticationDto
			{
				Email = appleUserForRegistration.Email,
				DeviceId = appleUserForRegistration.DeviceId,
				AppleId = appleUserForRegistration.AppleId,
			});
		}

		public async Task<TokenDto> RefreshTokenAsync(TokenForRefreshDto tokenDto)
		{
			var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken!);
			var userIdClaim = principal.FindFirst("UserId")?.Value;
			if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
			{
				throw new InvalidTokenBadRequestException();
			}

			var user = await _repository.User.GetByIdAsync(
				userId,
				trackChanges: true,
				includes: u => u.Include(u =>
					u.RefreshTokens.Where(rt => rt.Token == tokenDto.RefreshToken && rt.DeviceId == tokenDto.DeviceId)))
				?? throw new UserNotFoundException();

			var refreshToken = user.RefreshTokens.FirstOrDefault() ?? throw new RefreshTokenNotFoundException();

			var newRefreshToken = GenerateRefreshToken(user, tokenDto.DeviceId!);
			refreshToken.Token = newRefreshToken.Token;
			refreshToken.ExpirationDate = newRefreshToken.ExpirationDate;

			await _repository.SaveAsync();

			return new TokenDto
			{
				AccessToken = GenerateAccessToken(user),
				RefreshToken = refreshToken.Token!,
			};
		}

		public async Task ChangePasswordAsync(Guid userId, ChangePasswordDto passwordDto, bool trackChanges)
		{
			if (passwordDto.Password == passwordDto.NewPassword)
				throw new InvalidPasswordBadRequestException("Nova šifra ne može biti ista kao stara šifra.");

			var user = await _repository.User.GetByIdAsync(
				userId,
				trackChanges,
				includes: u => u.Include(u => u.RefreshTokens))
				?? throw new UserNotFoundException();

			if (!VerifyHash(passwordDto.Password!, user.PasswordHash!, user.PasswordSalt!))
				throw new InvalidPasswordBadRequestException();

			(user.PasswordHash, user.PasswordSalt) = CreateHash(passwordDto.NewPassword!);

			user.RefreshTokens.Clear();

			await _repository.SaveAsync();
		}

		public async Task GenerateTemporaryPasswordAsync(string email, bool trackChanges)
		{
			if (!IsValidEmail(email))
				throw new InvalidEmailAddressBadRequestException(email);

			var user = await _repository.User.GetByEmailAsync(email, trackChanges)
				?? throw new UserNotFoundException();

			var tempPassword = GeneratePassword();

			(user.TempPasswordHash, user.TempPasswordSalt) = CreateHash(tempPassword);
			user.TempPasswordExpiry = DateTimeHelper.GetUnspecifiedUtcNow().AddHours(1);

			await _repository.SaveAsync();

			await _emailSender.SendTemporaryPasswordAsync(email, tempPassword);
		}

		public async Task ChangePasswordWithTemporaryAsync(ChangePasswordWithTemporaryDto passwordDto, bool trackChanges)
		{
			if (!IsValidEmail(passwordDto.Email!))
				throw new InvalidEmailAddressBadRequestException(passwordDto.Email!);

			var user = await _repository.User.GetByEmailAsync(
				passwordDto.Email!,
				trackChanges,
				includes: u => u.Include(u => u.RefreshTokens))
				?? throw new UserNotFoundException();

			if (user.TempPasswordExpiry == null || user.TempPasswordExpiry < DateTimeHelper.GetUnspecifiedUtcNow())
				throw new InvalidTemporaryPasswordBadRequestException("Privremena šifra je istekla. Molimo vas da zatražite novu lozinku.");

			if (!VerifyHash(passwordDto.TemporaryPassword!, user.TempPasswordHash!, user.TempPasswordSalt!))
				throw new InvalidTemporaryPasswordBadRequestException("Privremena šifra je pogrešna.");

			(user.PasswordHash, user.PasswordSalt) = CreateHash(passwordDto.NewPassword!);

			user.TempPasswordHash = null;
			user.TempPasswordSalt = null;
			user.TempPasswordExpiry = null;

			if (user.RefreshTokens.Count != 0)
				user.RefreshTokens.Clear();

			await _repository.SaveAsync();
		}

		private static (string Hash, string Salt) CreateHash(string password)
		{
			using var hmac = new HMACSHA512();
			var salt = Convert.ToBase64String(hmac.Key);
			var hash = Convert.ToBase64String(
				hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

			return (hash, salt);
		}

		private static bool VerifyHash(string password, string storedHash, string storedSalt)
		{
			var saltBytes = Convert.FromBase64String(storedSalt);
			using var hmac = new HMACSHA512(saltBytes);
			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

			return Convert.ToBase64String(computedHash) == storedHash;
		}

		private static bool IsValidEmail(string email)
		{
			try
			{
				var addr = new MailAddress(email);
				return addr.Address == email;
			}
			catch (Exception ex)
			{
				Log.Warning(ex, "Invalid email format detected: {Email}", email);
				return false;
			}
		}

		private static string GenerateRandomToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		private static string GeneratePassword()
		{
			const string validChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
			return RandomNumberGenerator.GetString(validChars, 8);
		}

		private string GenerateAccessToken(User user)
		{
			var claims = new List<Claim>
			{
				new("UserId", Convert.ToString(user.Id)!),
				new("Email", user.Email ?? string.Empty),
				new("FirstName", user.FirstName ?? string.Empty),
				new("LastName", user.LastName ?? string.Empty),
				new("PhoneNumber", user.PhoneNumber ?? string.Empty),
				new("RegistrationType", Convert.ToString(user.RegistrationType)!),
			};

			var appSettingsToken = _jwtConfiguration.SecretKey;

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.Expires),
				SigningCredentials = creds,
				Issuer = _jwtConfiguration.ValidIssuer,
				Audience = _jwtConfiguration.ValidAudience,
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		private RefreshToken GenerateRefreshToken(User user, string deviceId)
		{
			var expirationDays = _jwtConfiguration.RefreshTokenExpirationDays;

			return new RefreshToken
			{
				Token = GenerateRandomToken(),
				ExpirationDate = DateTimeHelper.GetUnspecifiedUtcNow().AddDays(expirationDays),
				UserId = user.Id,
				DeviceId = deviceId,
			};
		}

		private async Task<RefreshToken> GetOrCreateRefreshTokenAsync(Guid userId, string deviceId)
		{
			var existingToken = await _repository.RefreshToken.GetRefreshTokenAsync(userId, deviceId, trackChanges: true);
			if (existingToken != null && existingToken.ExpirationDate > DateTimeHelper.GetUnspecifiedUtcNow())
				return existingToken;

			var newRefreshToken = GenerateRefreshToken(new User { Id = userId }, deviceId);

			if (existingToken != null)
			{
				existingToken.Token = newRefreshToken.Token;
				existingToken.ExpirationDate = newRefreshToken.ExpirationDate;

				await _repository.SaveAsync();
				return existingToken;
			}

			_repository.RefreshToken.CreateRefreshToken(newRefreshToken);

			await _repository.SaveAsync();
			return newRefreshToken;
		}

		private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidAlgorithms = [SecurityAlgorithms.HmacSha512],
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey!)),
				ValidateLifetime = false,
				ValidIssuer = _jwtConfiguration.ValidIssuer,
				ValidAudience = _jwtConfiguration.ValidAudience,
				RequireSignedTokens = true,
				TryAllIssuerSigningKeys = true,
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}

		private async Task CheckUserExistence(string email, string phoneNumber)
		{
			var user = await _repository.User.GetByEmailOrPhoneAsync(email, phoneNumber, trackChanges: false);
			if (user == null)
				return;

			if (user.Email == email)
				throw new UserExistsConflictException($"Korisnik sa email adresom '{email}' već postoji.");

			if (user.PhoneNumber == phoneNumber)
				throw new UserExistsConflictException($"Korisnik sa brojem telefona '{phoneNumber}' već postoji.");
		}
	}
}
