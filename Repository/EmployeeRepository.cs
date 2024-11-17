namespace UltimateDotNetSkeleton.Repository
{
	using UltimateDotNetSkeleton.Contracts;
	using UltimateDotNetSkeleton.Models;

	public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(RepositoryContext repositoryContext)
			: base(repositoryContext)
		{

		}
	}
}
