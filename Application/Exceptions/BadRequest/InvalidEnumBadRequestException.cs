namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidEnumBadRequestException : BadRequestException
	{
		public InvalidEnumBadRequestException(string message)
			: base(message)
		{
		}

		public InvalidEnumBadRequestException(string value, string enumName)
			: base($"Vrijednost '{value}' nije validna za enum {enumName}")
		{
		}
	}
}
