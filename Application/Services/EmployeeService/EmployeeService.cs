namespace UltimateDotNetSkeleton.Application.Services.EmployeeService
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using UltimateDotNetSkeleton.Application.DTOs.Employee;
    using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;
    using UltimateDotNetSkeleton.Application.Exceptions.NotFound;
    using UltimateDotNetSkeleton.Application.RequestFeatures;
    using UltimateDotNetSkeleton.Domain.Models;
    using UltimateDotNetSkeleton.Domain.Repositories.Manager;

    internal sealed class EmployeeService : IEmployeeService
	{
		private readonly IRepositoryManager _repository;
		private readonly IMapper _mapper;

		public EmployeeService(IRepositoryManager repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
		{
			await CheckIfCompanyExists(companyId, trackChanges);

			var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

			var employee = _mapper.Map<EmployeeDto>(employeeDb);

			return employee;
		}

		public async Task<(IEnumerable<EmployeeDto> Employees, MetaData MetaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
		{
			if (!employeeParameters.ValidAgeRange)
			{
				throw new MaxAgeRangeBadRequestException();
			}

			await CheckIfCompanyExists(companyId, trackChanges);

			var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);

			var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

			return (employeesDto, employeesWithMetaData.MetaData);
		}

		public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
		{
			await CheckIfCompanyExists(companyId, trackChanges);

			var employeeEntity = _mapper.Map<Employee>(employee);

			_repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
			await _repository.SaveAsync();

			var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

			return employeeToReturn;
		}

		public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
		{
			await CheckIfCompanyExists(companyId, compTrackChanges);

			var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

			_mapper.Map(employeeForUpdate, employeeEntity);
			await _repository.SaveAsync();
		}

		public async Task<(EmployeeForUpdateDto EmployeeToPatch, Employee EmployeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
		{
			await CheckIfCompanyExists(companyId, compTrackChanges);

			var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

			var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb);

			return (employeeToPatch, employeeDb);
		}

		public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
		{
			_mapper.Map(employeeToPatch, employeeEntity);
			await _repository.SaveAsync();
		}

		public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
		{
			await CheckIfCompanyExists(companyId, trackChanges);

			var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);

			_repository.Employee.DeleteEmployee(employeeForCompany);
			await _repository.SaveAsync();
		}

		private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
		{
			var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}
		}

		private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
		{
			var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges)
				?? throw new EmployeeNotFoundException(id);

			return employee;
		}
	}
}
