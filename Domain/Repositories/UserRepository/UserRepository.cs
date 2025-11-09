namespace UltimateDotNetSkeleton.Domain.Repositories.UserRepository
{
	using Microsoft.EntityFrameworkCore;

	using UltimateDotNetSkeleton.Domain.Context;
	using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Domain.Repositories.Base;
	using UltimateDotNetSkeleton.Domain.Repositories.Extensions.Utility;

	public class UserRepository : RepositoryBase<User>, IUserRepository
	{
		public UserRepository(RepositoryContext context)
			: base(context)
		{
		}

		public async Task<User?> GetByIdAsync(Guid id, bool trackChanges = false, params Func<IQueryable<User>, IQueryable<User>>[] includes) =>
			await FindByCondition(u => u.Id == id, trackChanges)
				.ApplyIncludes(includes)
				.SingleOrDefaultAsync();

		public async Task<User?> GetByEmailAsync(string email, bool trackChanges = false, params Func<IQueryable<User>, IQueryable<User>>[] includes) =>
			await FindByCondition(u => u.Email == email, trackChanges)
				.ApplyIncludes(includes)
				.SingleOrDefaultAsync();

		public async Task<User?> GetByPhoneAsync(string phone, bool trackChanges = false, params Func<IQueryable<User>, IQueryable<User>>[] includes) =>
			await FindByCondition(u => u.PhoneNumber == phone, trackChanges)
				.ApplyIncludes(includes)
				.SingleOrDefaultAsync();

		public async Task<User?> GetByEmailOrPhoneAsync(string email, string phoneNumber, bool trackChanges = false) =>
			await FindByCondition(u => u.Email == email || u.PhoneNumber == phoneNumber, trackChanges)
				.FirstOrDefaultAsync();

		public void CreateUser(User user) => Create(user);
	}
}
