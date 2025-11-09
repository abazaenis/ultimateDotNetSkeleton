namespace UltimateDotNetSkeleton.Presentation.Controllers
{
	using System.Net;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.RateLimiting;

	using UltimateDotNetSkeleton.Application.DTOs.Authentication;
	using UltimateDotNetSkeleton.Application.Services.Manager;
	using UltimateDotNetSkeleton.Infrastructure.Extensions;
	using UltimateDotNetSkeleton.Presentation.ActionFilters;

	[Route("api/authentication")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IServiceManager _service;

		public AuthenticationController(IServiceManager service)
		{
			_service = service;
		}

		[HttpPost("login")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
		{
			var authResponse = await _service.AuthenticationService.LoginAsync(user);

			return Ok(authResponse);
		}

		[HttpPost("login/google")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> GoogleAuthenticate([FromBody] GoogleUserForAuthenticationDto user)
		{
			var authResponse = await _service.AuthenticationService.GoogleLoginAsync(user);

			return Ok(authResponse);
		}

		[HttpPost("login/apple")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> AppleAuthenticate([FromBody] AppleUserForAuthenticationDto user)
		{
			var authResponse = await _service.AuthenticationService.AppleLoginAsync(user);

			return Ok(authResponse);
		}

		[HttpPost("register")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> Register([FromBody] UserForRegistrationDto user)
		{
			await _service.AuthenticationService.RegisterAsync(user);

			return StatusCode((int)HttpStatusCode.Created);
		}

		[HttpPost("register/google")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> GoogleRegister([FromBody] GoogleUserForRegistrationDto user)
		{
			var authResponse = await _service.AuthenticationService.GoogleRegisterAsync(user);

			return Ok(authResponse);
		}

		[HttpPost("register/apple")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> AppleRegister([FromBody] AppleUserForRegistrationDto user)
		{
			var authResponse = await _service.AuthenticationService.AppleRegisterAsync(user);

			return Ok(authResponse);
		}

		[Authorize]
		[HttpPost("change-password")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto passwordDto)
		{
			var userId = User.GetUserId();

			await _service.AuthenticationService.ChangePasswordAsync(userId, passwordDto, trackChanges: true);

			return NoContent();
		}

		[HttpPost("forgot-password")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		[EnableRateLimiting("ForgotPasswordPolicy")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto passwordDto)
		{
			await _service.AuthenticationService.GenerateTemporaryPasswordAsync(passwordDto.Email!, trackChanges: true);

			return NoContent();
		}

		[HttpPost("change-password-temporary")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> ChangePasswordWithTemporary([FromBody] ChangePasswordWithTemporaryDto passwordDto)
		{
			await _service.AuthenticationService.ChangePasswordWithTemporaryAsync(passwordDto, trackChanges: true);
			return NoContent();
		}
	}
}
