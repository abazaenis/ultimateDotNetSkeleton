namespace UltimateDotNetSkeleton.Application.Services.AuthenticationService
{
	using System.IdentityModel.Tokens.Jwt;
	using System.Security.Claims;
	using System.Security.Cryptography;
	using System.Text;

	using AutoMapper;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.IdentityModel.Tokens;

	using Serilog;

	using UltimateDotNetSkeleton.Application.DTOs.Token;
	using UltimateDotNetSkeleton.Application.DTOs.User;
	using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;
	using UltimateDotNetSkeleton.Domain.ConfigurationModels;
	using UltimateDotNetSkeleton.Domain.Models;

	public class AuthenticationService : IAuthenticationService
	{
		private readonly IMapper _mapper;

		private readonly UserManager<User> _userManager;

		private readonly RoleManager<IdentityRole> _roleManager;

		private readonly IConfiguration _configuration;

		private readonly JwtConfiguration _jwtConfiguration;

		private User? _user;

		public AuthenticationService(IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_mapper = mapper;
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_jwtConfiguration = new JwtConfiguration();
			_configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
		}

		public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
		{
			var existingRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

			var invalidRoles = userForRegistration.Roles!.Except(existingRoles).ToList();

			if (invalidRoles.Count != 0)
			{
				throw new InvalidRoleException(invalidRoles);
			}

			var user = _mapper.Map<User>(userForRegistration);

			var result = await _userManager.CreateAsync(user, userForRegistration.Password!);

			if (result.Succeeded)
			{
				await _userManager.AddToRolesAsync(user, userForRegistration.Roles!);
			}

			return result;
		}

		public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
		{
			_user = await _userManager.FindByNameAsync(userForAuth.UserName);

			var result = _user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password);

			if (!result)
			{
				Log.Warning($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");
			}

			return result;
		}

		public async Task<TokenDto> CreateToken(bool populateExp)
		{
			var signingCredentials = GetSigningCredentials();
			var claims = await GetClaims();
			var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

			var refreshToken = GenerateRefreshToken();
			_user.RefreshToken = refreshToken;

			if (populateExp)
			{
				_user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
			}

			await _userManager.UpdateAsync(_user);

			var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			return new TokenDto(accessToken, refreshToken);
		}

		public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
		{
			var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
			var user = await _userManager.FindByNameAsync(principal.Identity.Name);

			if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
			{
				throw new RefreshTokenBadRequestException();
			}

			_user = user;

			return await CreateToken(populateExp: false);
		}

		private static string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		private SigningCredentials GetSigningCredentials()
		{
			var key = Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey!);

			var secret = new SymmetricSecurityKey(key);

			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}

		private async Task<List<Claim>> GetClaims()
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, _user.UserName),
			};

			var roles = await _userManager.GetRolesAsync(_user);

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			return claims;
		}

		private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");

			var tokenOptions = new JwtSecurityToken(
				issuer: _jwtConfiguration.ValidIssuer,
				audience: _jwtConfiguration.ValidAudience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
				signingCredentials: signingCredentials);

			return tokenOptions;
		}

		private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey!)),
				ValidateLifetime = false,
				ValidIssuer = _jwtConfiguration.ValidIssuer,
				ValidAudience = _jwtConfiguration.ValidAudience,
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

			var jwtSecurityToken = securityToken as JwtSecurityToken;

			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}
	}
}
