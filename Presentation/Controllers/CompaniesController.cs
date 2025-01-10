namespace UltimateDotNetSkeleton.Presentation.Controllers
{
	using Microsoft.AspNetCore.JsonPatch;
	using Microsoft.AspNetCore.Mvc;

	using UltimateDotNetSkeleton.Application.DataTransferObjects.Company;
	using UltimateDotNetSkeleton.Application.DataTransferObjects.Employee;
	using UltimateDotNetSkeleton.Application.Manager;
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

		[HttpGet("collection/({ids})", Name = "CompanyCollection")]
		public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
		{
			var companies = _service.CompanyService.GetByIds(ids, trackChanges: false);

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

		[HttpPost("collection")]
		public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
		{
			var result = _service.CompanyService.CreateCompanyCollection(companyCollection);

			return CreatedAtRoute("CompanyCollection", new { result.Ids }, result.Companies);
		}

		[HttpPut("{id:guid}")]
		public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
		{
			if (company is null)
			{
				return BadRequest("CompanyForUpdateDto object is null");
			}

			_service.CompanyService.UpdateCompany(id, company, trackChanges: true);

			return NoContent();
		}

		[HttpPatch("{id:guid}")]
		public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
		{
			if (patchDoc is null)
			{
				return BadRequest("PatchDoc object sent from client is null.");
			}

			var result = _service.EmployeeService.GetEmployeeForPatch(companyId, id, compTrackChanges: false, empTrackChanges: true);

			patchDoc.ApplyTo(result.EmployeeToPatch);

			_service.EmployeeService.SaveChangesForPatch(result.EmployeeToPatch, result.EmployeeEntity);

			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		public IActionResult DeleteCompany(Guid id)
		{
			_service.CompanyService.DeleteCompany(id, trackChanges: false);

			return NoContent();
		}
	}
}
