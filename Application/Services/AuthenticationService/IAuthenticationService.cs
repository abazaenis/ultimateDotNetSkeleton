namespace UltimateDotNetSkeleton.Application.Services.AuthenticationService
{
	using Microsoft.AspNetCore.Identity;
	using UltimateDotNetSkeleton.Application.DTOs.User;

	public interface IAuthenticationService
	{
		Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);

		Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);

		Task<string> CreateToken();
	}
}
