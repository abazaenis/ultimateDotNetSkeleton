namespace UltimateDotNetSkeleton.Application.Exceptions.Forbidden
{
	public abstract class ForbiddenException : Exception
	{
		protected ForbiddenException(string message)
			: base(message)
		{
		}
	}
}
