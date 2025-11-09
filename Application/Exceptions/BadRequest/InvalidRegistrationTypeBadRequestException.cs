namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidRegistrationTypeBadRequestException : BadRequestException
	{
		public InvalidRegistrationTypeBadRequestException()
			: base("Prijava nije moguća - vaš nalog je registrovan preko eksternog provajdera.")
		{
		}

		public InvalidRegistrationTypeBadRequestException(string registrationProvider)
			: base($"Samo korisnici sa {registrationProvider} registracijom mogu koristiti ovu funkcionalnost.")
		{
		}
	}
}
