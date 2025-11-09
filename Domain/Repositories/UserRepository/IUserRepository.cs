namespace UltimateDotNetSkeleton.Domain.Repositories.UserRepository
{
	using UltimateDotNetSkeleton.Domain.Models;

	public interface IUserRepository
	{
		Task<User?> GetByIdAsync(Guid id, bool trackChanges = false, params Func<IQueryable<User>, IQueryable<User>>[] includes);

		Task<User?> GetByEmailAsync(string email, bool trackChanges = false, params Func<IQueryable<User>, IQueryable<User>>[] includes);

		Task<User?> GetByPhoneAsync(string phone, bool trackChanges = false, params Func<IQueryable<User>, IQueryable<User>>[] includes);

		Task<User?> GetByEmailOrPhoneAsync(string email, string phoneNumber, bool trackChanges = false);

		void CreateUser(User user);
	}
}
