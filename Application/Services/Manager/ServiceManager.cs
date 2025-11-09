namespace UltimateDotNetSkeleton.Application.Services.Manager
{
	using AutoMapper;
	using Microsoft.Extensions.Configuration;
	using UltimateDotNetSkeleton.Application.Services.AuthenticationService;
	using UltimateDotNetSkeleton.Application.Services.CompanyService;
	using UltimateDotNetSkeleton.Application.Services.EmployeeService;
	using UltimateDotNetSkeleton.Domain.Repositories.Manager;
	using UltimateDotNetSkeleton.Infrastructure.Services.EmailSender;

	public sealed class ServiceManager : IServiceManager
	{
		private readonly Lazy<ICompanyService> _companyService;
		private readonly Lazy<IEmployeeService> _employeeService;
		private readonly Lazy<IAuthenticationService> _authenticationService;

		public ServiceManager(
			IRepositoryManager repositoryManager,
			IMapper mapper,
			IEmailSender emailSender,
			IConfiguration configuration)
		{
			_companyService = new Lazy<ICompanyService>(() =>
				new CompanyService(repositoryManager, mapper));
			_employeeService = new Lazy<IEmployeeService>(() =>
				new EmployeeService(repositoryManager, mapper));
			_authenticationService = new Lazy<IAuthenticationService>(() =>
				new AuthenticationService(repositoryManager, mapper, emailSender, configuration));
		}

		public ICompanyService CompanyService => _companyService.Value;

		public IEmployeeService EmployeeService => _employeeService.Value;

		public IAuthenticationService AuthenticationService => _authenticationService.Value;
	}
}