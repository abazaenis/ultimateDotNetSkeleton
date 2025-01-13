namespace UltimateDotNetSkeleton.Application.Services.Manager
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Configuration;

	using UltimateDotNetSkeleton.Application.Services.AuthenticationService;
	using UltimateDotNetSkeleton.Application.Services.CompanyService;
	using UltimateDotNetSkeleton.Application.Services.EmployeeService;
	using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Domain.Repositories.Manager;
	using UltimateDotNetSkeleton.Infrastructure.Logger;

	public sealed class ServiceManager : IServiceManager
	{
		private readonly Lazy<ICompanyService> _companyService;
		private readonly Lazy<IEmployeeService> _employeeService;
		private readonly Lazy<IAuthenticationService> _authenticationService;

		public ServiceManager(
			IRepositoryManager repositoryManager,
			ILoggerManager logger,
			IMapper mapper,
			UserManager<User> userManager,
			RoleManager<IdentityRole> roleManager,
			IConfiguration configuration)
		{
			_companyService = new Lazy<ICompanyService>(() =>
				new CompanyService(repositoryManager, logger, mapper));
			_employeeService = new Lazy<IEmployeeService>(() =>
				new EmployeeService(repositoryManager, logger, mapper));
			_authenticationService = new Lazy<IAuthenticationService>(() =>
				new AuthenticationService(logger, mapper, userManager, roleManager, configuration));
		}

		public ICompanyService CompanyService => _companyService.Value;

		public IEmployeeService EmployeeService => _employeeService.Value;

		public IAuthenticationService AuthenticationService => _authenticationService.Value;
	}
}