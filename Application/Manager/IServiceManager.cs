namespace UltimateDotNetSkeleton.Application.Manager
{
    using UltimateDotNetSkeleton.Application.Services.CompanyService;
    using UltimateDotNetSkeleton.Application.Services.EmployeeService;

    public interface IServiceManager
    {
        ICompanyService CompanyService { get; }

        IEmployeeService EmployeeService { get; }
    }
}
