namespace UltimateDotNetSkeleton.Presentation.Controllers
{
	using Microsoft.AspNetCore.JsonPatch;
	using Microsoft.AspNetCore.Mvc;
	using UltimateDotNetSkeleton.Application.DataTransferObjects.Employee;
	using UltimateDotNetSkeleton.Application.Manager;

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

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var employeeToReturn = _service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee is null)
			{
				return BadRequest("EmployeeForUpdateDto object is null");
			}

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, compTrackChanges: false, empTrackChanges: true);

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

			patchDoc.ApplyTo(result.EmployeeToPatch, ModelState);

			TryValidateModel(result.EmployeeToPatch);

			if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

			_service.EmployeeService.SaveChangesForPatch(result.EmployeeToPatch, result.EmployeeEntity);

			return NoContent();
		}

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            _service.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);

            return NoContent();
        }
	}
}
