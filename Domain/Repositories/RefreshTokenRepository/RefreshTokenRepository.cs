namespace UltimateDotNetSkeleton.Domain.Repositories.RefreshTokenRepository
{
    using Microsoft.EntityFrameworkCore;
    using UltimateDotNetSkeleton.Domain.Context;
    using UltimateDotNetSkeleton.Domain.Models;
    using UltimateDotNetSkeleton.Domain.Repositories.Base;

    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
	{
		public RefreshTokenRepository(RepositoryContext repositoryContext)
			: base(repositoryContext)
		{
		}

		public async Task<RefreshToken?> GetRefreshTokenAsync(Guid userId, string deviceId, bool trackChanges) =>
			await FindByCondition(t => t.UserId.Equals(userId) && t.DeviceId.Equals(deviceId), trackChanges).FirstOrDefaultAsync();

		public void CreateRefreshToken(RefreshToken token) => Create(token);
	}
}
