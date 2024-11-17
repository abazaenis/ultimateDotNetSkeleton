namespace UltimateDotNetSkeleton.Repositories.Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }

        IEmployeeRepository Employee { get; }

        void Save();
    }
}
