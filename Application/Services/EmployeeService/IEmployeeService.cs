namespace UltimateDotNetSkeleton.Application.Services.EmployeeService
{
	using UltimateDotNetSkeleton.Application.DataTransferObjects.Employee;
	using UltimateDotNetSkeleton.Domain.Models;

	public interface IEmployeeService
	{
		EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);

		IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);

		EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);

		void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);

		(EmployeeForUpdateDto EmployeeToPatch, Employee EmployeeEntity) GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

		void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);

		void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
	}
}
