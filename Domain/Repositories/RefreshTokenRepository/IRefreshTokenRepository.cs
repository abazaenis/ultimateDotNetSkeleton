namespace UltimateDotNetSkeleton.Domain.Repositories.RefreshTokenRepository
{
	using UltimateDotNetSkeleton.Domain.Models;

	public interface IRefreshTokenRepository
	{
		void CreateRefreshToken(RefreshToken token);

		Task<RefreshToken?> GetRefreshTokenAsync(Guid userId, string deviceId, bool trackChanges);
	}
}
