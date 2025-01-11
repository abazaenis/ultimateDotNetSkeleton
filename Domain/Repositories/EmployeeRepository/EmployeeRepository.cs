﻿namespace UltimateDotNetSkeleton.Domain.Repositories.EmployeeRepository
{
	using System;
	using System.Collections.Generic;

	using Microsoft.EntityFrameworkCore;

	using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Domain.Repositories.Base;
	using UltimateDotNetSkeleton.Domain.Repositories.Context;
	using UltimateDotNetSkeleton.Shared.RequestFeatures;

	public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(RepositoryContext repositoryContext)
			: base(repositoryContext)
		{
		}

		public async Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
			await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

		public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
		{
			var employees = await FindByCondition(
				e => e.CompanyId.Equals(companyId) &&
				e.Age >= employeeParameters.MinAge &&
				e.Age <= employeeParameters.MaxAge,
				trackChanges)
				.OrderBy(e => e.Name)
				.Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
				.Take(employeeParameters.PageSize)
				.ToListAsync();

			var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();

			return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
		}

		public void CreateEmployeeForCompany(Guid companyId, Employee employee)
		{
			employee.CompanyId = companyId;
			Create(employee);
		}

		public void DeleteEmployee(Employee employee) => Delete(employee);
	}
}
