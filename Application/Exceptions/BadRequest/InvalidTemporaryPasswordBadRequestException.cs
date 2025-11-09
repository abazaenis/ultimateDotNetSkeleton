namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidTemporaryPasswordBadRequestException : BadRequestException
	{
		public InvalidTemporaryPasswordBadRequestException(string message)
			: base(message)
		{
		}
	}
}
