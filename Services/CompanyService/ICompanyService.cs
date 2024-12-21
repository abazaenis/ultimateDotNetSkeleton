namespace UltimateDotNetSkeleton.Services.CompanyService
{
    using UltimateDotNetSkeleton.Dtos.Company;

    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);

        CompanyDto GetCompany(Guid companyId, bool trackChanges);

        IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        CompanyDto CreateCompany(CompanyForCreationDto company, bool trackChanges);

        (IEnumerable<CompanyDto> Companies, string Ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
	}
}
