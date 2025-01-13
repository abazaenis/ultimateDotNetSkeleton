namespace UltimateDotNetSkeleton.Presentation.Controllers
{
	using Microsoft.AspNetCore.Mvc;

	using UltimateDotNetSkeleton.Application.DTOs.User;
	using UltimateDotNetSkeleton.Application.Services.Manager;
	using UltimateDotNetSkeleton.Presentation.ActionFilters;

	[Route("api/[controller]")]
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
			if (!await _service.AuthenticationService.ValidateUser(user))
			{
				return Unauthorized();
			}

			return Ok(new
			{
				Token = await _service.AuthenticationService.CreateToken(),
			});
		}

		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
		{
			var result = await _service.AuthenticationService.RegisterUser(userForRegistration);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.TryAddModelError(error.Code, error.Description);
				}

				return BadRequest(ModelState);
			}

			return StatusCode(201);
		}
	}
}
