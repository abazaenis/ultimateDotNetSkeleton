namespace UltimateDotNetSkeleton.Services.Contracts
{
	using UltimateDotNetSkeleton.Dtos.Employee;

	public interface IEmployeeService
    {
        EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);

        IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);

        EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges);
    }
}
