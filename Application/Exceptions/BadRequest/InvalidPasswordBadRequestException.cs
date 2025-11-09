namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidPasswordBadRequestException : BadRequestException
	{
		public InvalidPasswordBadRequestException()
			: base("Pogrešan password.")
		{
		}

		public InvalidPasswordBadRequestException(string message)
			: base(message)
		{
		}
	}
}
