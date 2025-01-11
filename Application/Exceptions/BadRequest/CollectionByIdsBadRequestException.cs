namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class CollectionByIdsBadRequestException : BadRequestException
	{
		public CollectionByIdsBadRequestException()
			: base("Collection count mismatch comparing to ids.")
		{
		}
	}
}
