namespace UltimateDotNetSkeleton.Application.Services.AuthenticationService
{
	using UltimateDotNetSkeleton.Application.DTOs.Authentication;
	using UltimateDotNetSkeleton.Application.DTOs.Token;

	public interface IAuthenticationService
	{
		Task<TokenDto> LoginAsync(UserForAuthenticationDto userForAuthentication);

		Task<TokenDto> GoogleLoginAsync(GoogleUserForAuthenticationDto googleUserForAuthentication);

		Task<TokenDto> AppleLoginAsync(AppleUserForAuthenticationDto appleUserForAuthentication);

		Task<TokenDto> RefreshTokenAsync(TokenForRefreshDto tokenDto);

		Task RegisterAsync(UserForRegistrationDto userForRegistration);

		Task<TokenDto> GoogleRegisterAsync(GoogleUserForRegistrationDto googleUserForRegistration);

		Task<TokenDto> AppleRegisterAsync(AppleUserForRegistrationDto appleUserForRegistration);

		Task ChangePasswordAsync(Guid userId, ChangePasswordDto passwordDto, bool trackChanges);

		Task GenerateTemporaryPasswordAsync(string email, bool trackChanges);

		Task ChangePasswordWithTemporaryAsync(ChangePasswordWithTemporaryDto passwordDto, bool trackChanges);
	}
}
