namespace UltimateDotNetSkeleton.Controllers
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	using UltimateDotNetSkeleton.Services.Contracts;

	[Route("api/[controller]")]
	[ApiController]
	public class CompaniesController : ControllerBase
	{
		private readonly IServiceManager _service;

		public CompaniesController(IServiceManager service)
		{
			_service = service;
		}

		[HttpGet]
		public IActionResult GetCompanies()
		{
			try
			{
				var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);

				return Ok(companies);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
