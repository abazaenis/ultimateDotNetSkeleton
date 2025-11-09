namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidTokenBadRequestException : BadRequestException
	{
		public InvalidTokenBadRequestException()
			: base("Nije moguće izdvojiti principal iz access tokena.")
		{
		}
	}
}
