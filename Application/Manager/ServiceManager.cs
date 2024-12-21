namespace UltimateDotNetSkeleton.Application.Manager
{
    using AutoMapper;
    using UltimateDotNetSkeleton.Application.Services.CompanyService;
    using UltimateDotNetSkeleton.Application.Services.EmployeeService;
    using UltimateDotNetSkeleton.Domain.Repositories.Manager;
    using UltimateDotNetSkeleton.Infrastructure.Logger;

    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _companyService = new Lazy<ICompanyService>(() =>
                new CompanyService(repositoryManager, logger, mapper));
            _employeeService = new Lazy<IEmployeeService>(() =>
                new EmployeeService(repositoryManager, logger, mapper));
        }

        public ICompanyService CompanyService => _companyService.Value;

        public IEmployeeService EmployeeService => _employeeService.Value;
    }
}