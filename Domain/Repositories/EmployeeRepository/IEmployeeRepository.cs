namespace UltimateDotNetSkeleton.Domain.Repositories.EmployeeRepository
{
    using UltimateDotNetSkeleton.Application.RequestFeatures;
    using UltimateDotNetSkeleton.Domain.Models;

    public interface IEmployeeRepository
	{
		Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);

		Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

		void CreateEmployeeForCompany(Guid companyId, Employee employee);

		void DeleteEmployee(Employee employee);
	}
}
