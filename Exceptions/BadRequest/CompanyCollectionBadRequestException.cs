namespace UltimateDotNetSkeleton.Exceptions.BadRequest
{
	public sealed class CompanyCollectionBadRequestException : BadRequestException
	{
		public CompanyCollectionBadRequestException()
			: base("Company collection sent from a client is null.")
		{
		}
	}
}
