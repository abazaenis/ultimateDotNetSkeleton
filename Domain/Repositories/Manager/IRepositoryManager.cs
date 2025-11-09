namespace UltimateDotNetSkeleton.Domain.Repositories.Manager
{
	using UltimateDotNetSkeleton.Domain.Repositories.CompanyRepository;
	using UltimateDotNetSkeleton.Domain.Repositories.EmployeeRepository;
	using UltimateDotNetSkeleton.Domain.Repositories.RefreshTokenRepository;
	using UltimateDotNetSkeleton.Domain.Repositories.UserRepository;

	public interface IRepositoryManager
	{
		ICompanyRepository Company { get; }

		IEmployeeRepository Employee { get; }

		IUserRepository User { get; }

		IRefreshTokenRepository RefreshToken { get; }

		Task SaveAsync();
	}
}
