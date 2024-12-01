namespace UltimateDotNetSkeleton.Repositories.Contracts
{
	using UltimateDotNetSkeleton.Models;

	public interface ICompanyRepository
    {
		Company? GetCompany(Guid companyId, bool trackChanges);

		IEnumerable<Company> GetAllCompanies(bool trackChanges);

		void CreateCompany(Company company);
    }
}
