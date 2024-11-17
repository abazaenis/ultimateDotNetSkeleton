using UltimateDotNetSkeleton.Contracts;

namespace UltimateDotNetSkeleton.Repository.Manager
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }

        IEmployeeRepository Employee { get; }

        void Save();
    }
}
