namespace UltimateDotNetSkeleton.Domain.Repositories.Manager
{
    using UltimateDotNetSkeleton.Domain.Repositories.CompanyRepository;
    using UltimateDotNetSkeleton.Domain.Repositories.EmployeeRepository;

    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }

        IEmployeeRepository Employee { get; }

        void Save();
    }
}
