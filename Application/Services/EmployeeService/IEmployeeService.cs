namespace UltimateDotNetSkeleton.Application.Services.EmployeeService
{
    using UltimateDotNetSkeleton.Application.DTOs.Employee;
    using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Shared.RequestFeatures;

    public interface IEmployeeService
	{
		Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

		Task<(IEnumerable<EmployeeDto> Employees, MetaData MetaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);

		Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);

		Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);

		Task<(EmployeeForUpdateDto EmployeeToPatch, Employee EmployeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

		Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);

		Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);
	}
}
