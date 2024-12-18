﻿namespace UltimateDotNetSkeleton.Services.Implementations
{
	using System;
	using System.Collections.Generic;

	using AutoMapper;

	using UltimateDotNetSkeleton.Dtos.Employee;
	using UltimateDotNetSkeleton.Exceptions.NotFound;
	using UltimateDotNetSkeleton.Models;
	using UltimateDotNetSkeleton.Repositories.Contracts;
	using UltimateDotNetSkeleton.Services.Contracts;
	using UltimateDotNetSkeleton.Utilities.Logger;

	internal sealed class EmployeeService : IEmployeeService
	{
		private readonly IRepositoryManager _repository;
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;

		public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
		{
			var company = _repository.Company.GetCompany(companyId, trackChanges);
			if (company == null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employeeDb = _repository.Employee.GetEmployee(companyId, id, trackChanges);
			if (employeeDb == null)
			{
				throw new EmployeeNotFoundException(id);
			}

			var employee = _mapper.Map<EmployeeDto>(employeeDb);

			return employee;
		}

		public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
		{
			var company = _repository.Company.GetCompany(companyId, trackChanges);
			if (company == null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employeesFromDb = _repository.Employee.GetEmployees(companyId, trackChanges);

			var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

			return employeesDto;
		}

		public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
		{
			var company = _repository.Company.GetCompany(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

			_repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
			_repository.Save();

			var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

			return employeeToReturn;
		}
	}
}
