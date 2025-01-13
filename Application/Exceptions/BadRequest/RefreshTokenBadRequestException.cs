namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class RefreshTokenBadRequestException : BadRequestException
	{
		public RefreshTokenBadRequestException()
            : base("Invalid client request. The tokenDto has some invalid values.")
		{
		}
	}
}
