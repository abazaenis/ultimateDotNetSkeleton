namespace UltimateDotNetSkeleton.Application.Services.AuthenticationService
{
	using Microsoft.AspNetCore.Identity;

	using UltimateDotNetSkeleton.Application.DTOs.Token;
	using UltimateDotNetSkeleton.Application.DTOs.User;

	public interface IAuthenticationService
	{
		Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);

		Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);

		Task<TokenDto> CreateToken(bool populateExp);

		Task<TokenDto> RefreshToken(TokenDto tokenDto);
	}
}
