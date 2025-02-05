﻿namespace UltimateDotNetSkeleton.Presentation.Controllers
{
    using System.Text.Json;

    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using UltimateDotNetSkeleton.Application.DTOs.Employee;
    using UltimateDotNetSkeleton.Application.RequestFeatures;
    using UltimateDotNetSkeleton.Application.Services.Manager;
    using UltimateDotNetSkeleton.Presentation.ActionFilters;

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
		public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
		{
			var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);

			return Ok(employee);
		}

		[HttpGet]
		public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
		{
			var pagedResult = await _service.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

			Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.MetaData));

			return Ok(pagedResult.Employees);
		}

		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
		{
			var employeeToReturn = await _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);

			return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
		}

		[HttpPut("{id:guid}")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
		{
			await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee, compTrackChanges: false, empTrackChanges: true);

			return NoContent();
		}

		[HttpPatch("{id:guid}")]
		public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
		{
			if (patchDoc is null)
			{
				return BadRequest("PatchDoc object sent from client is null.");
			}

			var result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, compTrackChanges: false, empTrackChanges: true);

			patchDoc.ApplyTo(result.EmployeeToPatch, ModelState);

			TryValidateModel(result.EmployeeToPatch);

			if (!ModelState.IsValid)
			{
				return UnprocessableEntity(ModelState);
			}

			await _service.EmployeeService.SaveChangesForPatchAsync(result.EmployeeToPatch, result.EmployeeEntity);

			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
		{
			await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);

			return NoContent();
		}
	}
}
