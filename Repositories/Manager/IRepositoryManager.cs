namespace UltimateDotNetSkeleton.Repositories.Manager
{
	using UltimateDotNetSkeleton.Repositories.CompanyRepository;
	using UltimateDotNetSkeleton.Repositories.EmployeeRepository;

	public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }

        IEmployeeRepository Employee { get; }

        void Save();
    }
}
