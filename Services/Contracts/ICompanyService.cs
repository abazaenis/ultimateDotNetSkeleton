namespace UltimateDotNetSkeleton.Services.Contracts
{
	using UltimateDotNetSkeleton.Dtos.Company;

	public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);

        CompanyDto GetCompany(Guid companyId, bool trackChanges);

        CompanyDto CreateCompany(CompanyForCreationDto company, bool trackChanges);
    }
}
