namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidEmailAddressBadRequestException : BadRequestException
	{
		public InvalidEmailAddressBadRequestException(string email)
			: base($"Email adresa '{email}' nije validna.")
		{
		}
	}
}
