namespace UltimateDotNetSkeleton.Application.Exceptions.NotFound
{
	public sealed class UserNotFoundException : NotFoundException
	{
		public UserNotFoundException(string message)
			: base(message)
		{
		}

		public UserNotFoundException()
			: base("Korisnik nije pronađen.")
		{
		}
	}
}
