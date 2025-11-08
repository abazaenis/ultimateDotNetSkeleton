namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public class InvalidBlobUploadBadRequestException : BadRequestException
	{
		public InvalidBlobUploadBadRequestException(string message)
			: base(message)
		{
		}
	}
}
