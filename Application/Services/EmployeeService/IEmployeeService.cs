namespace UltimateDotNetSkeleton.Application.Services.EmployeeService
{
	using UltimateDotNetSkeleton.Application.DataTransferObjects.Employee;
	using UltimateDotNetSkeleton.Domain.Models;

	public interface IEmployeeService
	{
		Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

		Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges);

		Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);

		Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);

		Task<(EmployeeForUpdateDto EmployeeToPatch, Employee EmployeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

		Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);

		Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);
	}
}
