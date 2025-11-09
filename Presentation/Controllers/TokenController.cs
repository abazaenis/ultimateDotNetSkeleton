namespace UltimateDotNetSkeleton.Presentation.Controllers
{
	using Microsoft.AspNetCore.Mvc;

	using UltimateDotNetSkeleton.Application.DTOs.Token;
	using UltimateDotNetSkeleton.Application.Services.Manager;
	using UltimateDotNetSkeleton.Presentation.ActionFilters;

	[Route("api/token")]
	[ApiController]
	public class TokenController : ControllerBase
	{
		private readonly IServiceManager _service;

		public TokenController(IServiceManager service) => _service = service;

		[HttpPost("refresh")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> Refresh([FromBody] TokenForRefreshDto tokenDto)
		{
			var tokenDtoToReturn = await _service.AuthenticationService.RefreshTokenAsync(tokenDto);

			return Ok(tokenDtoToReturn);
		}
	}
}
