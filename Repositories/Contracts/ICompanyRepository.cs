namespace UltimateDotNetSkeleton.Repositories.Contracts
{
	using UltimateDotNetSkeleton.Models;

	public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
    }
}
