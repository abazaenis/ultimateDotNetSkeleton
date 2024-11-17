namespace UltimateDotNetSkeleton.Repository
{
	using UltimateDotNetSkeleton.Contracts;
	using UltimateDotNetSkeleton.Models;

	public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
	{
		public CompanyRepository(RepositoryContext repositoryContext)
			: base(repositoryContext)
		{
		}
	}
}
