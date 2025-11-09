namespace UltimateDotNetSkeleton.Domain.Repositories.Manager
{
    using UltimateDotNetSkeleton.Domain.Context;
    using UltimateDotNetSkeleton.Domain.Repositories.CompanyRepository;
    using UltimateDotNetSkeleton.Domain.Repositories.EmployeeRepository;
    using UltimateDotNetSkeleton.Domain.Repositories.RefreshTokenRepository;
    using UltimateDotNetSkeleton.Domain.Repositories.UserRepository;

    public sealed class RepositoryManager : IRepositoryManager
	{
		private readonly RepositoryContext _repositoryContext;
		private readonly Lazy<ICompanyRepository> _companyRepository;
		private readonly Lazy<IEmployeeRepository> _employeeRepository;
		private readonly Lazy<IUserRepository> _userRepository;
		private readonly Lazy<IRefreshTokenRepository> _refreshTokenRepository;

		public RepositoryManager(RepositoryContext repositoryContext)
		{
			_repositoryContext = repositoryContext;
			_companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
			_employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));
			_userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
			_refreshTokenRepository = new Lazy<IRefreshTokenRepository>(() => new RefreshTokenRepository(repositoryContext));
		}

		public ICompanyRepository Company => _companyRepository.Value;

		public IEmployeeRepository Employee => _employeeRepository.Value;

		public IUserRepository User => _userRepository.Value;

		public IRefreshTokenRepository RefreshToken => _refreshTokenRepository.Value;

		public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
	}
}
