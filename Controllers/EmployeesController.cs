namespace UltimateDotNetSkeleton.Controllers
{
	using Microsoft.AspNetCore.Mvc;

	using UltimateDotNetSkeleton.Dtos.Employee;
	using UltimateDotNetSkeleton.Services.Contracts;

	[Route("api/companies/{companyId}/employees")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly IServiceManager _service;

		public EmployeesController(IServiceManager service)
		{
			_service = service;
		}

		[HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
		public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
		{
			var employee = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);

			return Ok(employee);
		}

		[HttpGet]
		public IActionResult GetEmployeesForCompany(Guid companyId)
		{
			var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false);

			return Ok(employees);
		}

		[HttpPost]
		public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
		{
			if (employee is null)
			{
				return BadRequest("EmployeeForCreationDto object is null");
			}

			var employeeToReturn = _service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);

			return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
		}
	}
}
