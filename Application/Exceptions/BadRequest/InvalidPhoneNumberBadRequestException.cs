namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidPhoneNumberBadRequestException : BadRequestException
	{
		public InvalidPhoneNumberBadRequestException(string message)
			: base(message)
		{
		}

		public InvalidPhoneNumberBadRequestException()
			: base("Za slanje SMSa potreban je validan bosanski broj telefona u formatu +387XXXXXXX.")
		{
		}
	}
}
