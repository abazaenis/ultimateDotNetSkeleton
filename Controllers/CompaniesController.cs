namespace UltimateDotNetSkeleton.Controllers
{
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

		[HttpGet("GetCompany")]
		public IActionResult GetCompany(Guid id)
		{
			var company = _service.CompanyService.GetCompany(id, trackChanges: false);

			return Ok(company);
		}

		[HttpGet("GetCompanies")]
		public IActionResult GetCompanies()
		{
			var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);

			return Ok(companies);
		}
	}
}
