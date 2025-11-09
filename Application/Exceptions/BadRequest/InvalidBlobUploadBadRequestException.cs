namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidBlobUploadBadRequestException : BadRequestException
	{
		public InvalidBlobUploadBadRequestException(string message)
			: base(message)
		{
		}
	}
}
