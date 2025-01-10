namespace UltimateDotNetSkeleton.Application.Services.CompanyService
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using UltimateDotNetSkeleton.Application.DataTransferObjects.Company;
    using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;
    using UltimateDotNetSkeleton.Application.Exceptions.NotFound;
    using UltimateDotNetSkeleton.Domain.Models;
    using UltimateDotNetSkeleton.Domain.Repositories.Manager;
    using UltimateDotNetSkeleton.Infrastructure.Logger;

    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }

        public CompanyDto GetCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);

            var companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
            {
                throw new IdParametersBadRequestException();
            }

            var companyEntities = _repository.Company.GetByIds(ids, trackChanges);

            if (ids.Count() != companyEntities.Count())
            {
                throw new CollectionByIdsBadRequestException();
            }

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return companiesToReturn;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company, bool trackChanges)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            _repository.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;
        }

        public (IEnumerable<CompanyDto> Companies, string Ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
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

            _repository.Save();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

            return (Companies: companyCollectionToReturn, Ids: ids);
        }

        public void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var companyEntity = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);

            _mapper.Map(companyForUpdate, companyEntity);

            _repository.Save();
        }

        public void DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);

            _repository.Company.DeleteCompany(company);
            _repository.Save();
        }
    }
}
