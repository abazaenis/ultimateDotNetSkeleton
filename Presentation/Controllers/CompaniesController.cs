namespace UltimateDotNetSkeleton.Presentation.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.RateLimiting;

	using UltimateDotNetSkeleton.Application.DTOs.Company;
	using UltimateDotNetSkeleton.Application.Services.Manager;
	using UltimateDotNetSkeleton.Presentation.ActionFilters;
	using UltimateDotNetSkeleton.Presentation.ModelBinders;

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
		public async Task<IActionResult> GetCompany(Guid id)
		{
			var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);

			return Ok(company);
		}

		[HttpGet]
		[Authorize(Roles = "Manager")]
		[EnableRateLimiting("FixedTokenPolicy")]
		public async Task<IActionResult> GetCompanies()
		{
			var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);

			return Ok(companies);
		}

		[HttpGet("collection/({ids})", Name = "CompanyCollection")]
		public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
		{
			var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);

			return Ok(companies);
		}

		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
		{
			var createdCompany = await _service.CompanyService.CreateCompanyAsync(company, trackChanges: false);

			return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
		}

		[HttpPost("collection")]
		public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
		{
			var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);

			return CreatedAtRoute("CompanyCollection", new { result.Ids }, result.Companies);
		}

		[HttpPut("{id:guid}")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
		{
			await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);

			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteCompany(Guid id)
		{
			await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);

			return NoContent();
		}
	}
}
