namespace UltimateDotNetSkeleton.Application.Services.CompanyService
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using UltimateDotNetSkeleton.Application.DTOs.Company;
    using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;
    using UltimateDotNetSkeleton.Application.Exceptions.NotFound;
    using UltimateDotNetSkeleton.Domain.Models;
    using UltimateDotNetSkeleton.Domain.Repositories.Manager;

    internal sealed class CompanyService : ICompanyService
	{
		private readonly IRepositoryManager _repository;
		private readonly IMapper _mapper;

		public CompanyService(IRepositoryManager repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
		{
			var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);

			var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

			return companiesDto;
		}

		public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
		{
			var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

			var companyDto = _mapper.Map<CompanyDto>(company);

			return companyDto;
		}

		public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
		{
			if (ids is null)
			{
				throw new IdParametersBadRequestException();
			}

			var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges);

			if (ids.Count() != companyEntities.Count())
			{
				throw new CollectionByIdsBadRequestException();
			}

			var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

			return companiesToReturn;
		}

		public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company, bool trackChanges)
		{
			var companyEntity = _mapper.Map<Company>(company);

			_repository.Company.CreateCompany(companyEntity);
			await _repository.SaveAsync();

			var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

			return companyToReturn;
		}

		public async Task<(IEnumerable<CompanyDto> Companies, string Ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
		{
			if (companyCollection is null)
			{
				throw new CompanyCollectionBadRequestException();
			}

			var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

			foreach (var company in companyEntities)
			{
				_repository.Company.CreateCompany(company);
			}

			await _repository.SaveAsync();

			var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

			var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

			return (Companies: companyCollectionToReturn, Ids: ids);
		}

		public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
		{
			var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

			_mapper.Map(companyForUpdate, company);

			await _repository.SaveAsync();
		}

		public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
		{
			var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

			_repository.Company.DeleteCompany(company);
			await _repository.SaveAsync();
		}

		private async Task<Company> GetCompanyAndCheckIfItExists(Guid companyId, bool trackChanges)
		{
			var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			return company;
		}
	}
}
