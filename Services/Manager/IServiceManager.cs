namespace UltimateDotNetSkeleton.Services.Manager
{
	using UltimateDotNetSkeleton.Services.CompanyService;
	using UltimateDotNetSkeleton.Services.EmployeeService;

	public interface IServiceManager
    {
        ICompanyService CompanyService { get; }

        IEmployeeService EmployeeService { get; }
    }
}
