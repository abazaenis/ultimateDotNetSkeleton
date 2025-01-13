namespace UltimateDotNetSkeleton.Application.Services.Manager
{
	using UltimateDotNetSkeleton.Application.Services.AuthenticationService;
	using UltimateDotNetSkeleton.Application.Services.CompanyService;
	using UltimateDotNetSkeleton.Application.Services.EmployeeService;

	public interface IServiceManager
	{
		ICompanyService CompanyService { get; }

		IEmployeeService EmployeeService { get; }

		IAuthenticationService AuthenticationService { get; }
	}
}
