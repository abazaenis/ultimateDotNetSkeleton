namespace UltimateDotNetSkeleton.Application.Exceptions.ServiceUnavailable
{
	public sealed class EmailServiceUnavailableException : ServiceUnavailableException
	{
		public EmailServiceUnavailableException()
			: base("Email servis trenutno nije dostupan. Molimo pokušajte kasnije.")
		{
		}
	}
}
