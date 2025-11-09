namespace UltimateDotNetSkeleton.Application.Exceptions.NotFound
{
	public sealed class RefreshTokenNotFoundException : NotFoundException
	{
		public RefreshTokenNotFoundException()
			: base("Refresh token nije pronađen")
		{
		}
	}
}