namespace UltimateDotNetSkeleton.Repositories.Implementations.Employee
{
	using System;
	using System.Collections.Generic;

	using UltimateDotNetSkeleton.Models;
	using UltimateDotNetSkeleton.Repositories.Contracts;
	using UltimateDotNetSkeleton.Repositories.Implementations.Base;
	using UltimateDotNetSkeleton.Repository.Context;

	public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(RepositoryContext repositoryContext)
			: base(repositoryContext)
		{
		}

		public Employee? GetEmployee(Guid companyId, Guid id, bool trackChanges)
		{
			return FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefault();
		}

		public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
		{
			return FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).OrderBy(e => e.Name).ToList();
		}

		public void CreateEmployeeForCompany(Guid companyId, Employee employee)
		{
			employee.CompanyId = companyId;
			Create(employee);
		}
	}
}
