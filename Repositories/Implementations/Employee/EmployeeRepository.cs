namespace UltimateDotNetSkeleton.Repositories.Implementations.Employee
{
    using UltimateDotNetSkeleton.Models;
    using UltimateDotNetSkeleton.Repositories.Contracts;
    using UltimateDotNetSkeleton.Repositories.Implementations.Base;
    using UltimateDotNetSkeleton.Repository.Context;

    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
