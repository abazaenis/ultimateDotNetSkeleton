namespace UltimateDotNetSkeleton.Controllers
{
	using Microsoft.AspNetCore.Mvc;

	using UltimateDotNetSkeleton.Dtos.Company;
	using UltimateDotNetSkeleton.Services.Contracts;

	[Route("api/companies")]
	[ApiController]
	public class CompaniesController : ControllerBase
	{
		private readonly IServiceManager _service;

		public CompaniesController(IServiceManager service)
		{
			_service = service;
		}

		[HttpGet("{id:guid}", Name = "CompanyById")]
		public IActionResult GetCompany(Guid id)
		{
			var company = _service.CompanyService.GetCompany(id, trackChanges: false);

			return Ok(company);
		}

		[HttpGet]
		public IActionResult GetCompanies()
		{
			var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);

			return Ok(companies);
		}

		[HttpPost]
		public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
		{
			if (company is null)
			{
				return BadRequest("CompanyForCreationDto object is null");
			}

			var createdCompany = _service.CompanyService.CreateCompany(company, trackChanges: false);

			return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
		}
	}
}
