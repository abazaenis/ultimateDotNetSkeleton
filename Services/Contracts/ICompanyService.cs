namespace UltimateDotNetSkeleton.Services.Contracts
{
	using UltimateDotNetSkeleton.Dtos.Company;

	public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    }
}
