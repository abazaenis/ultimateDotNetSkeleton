namespace UltimateDotNetSkeleton.Application.Exceptions.ServiceUnavailable
{
	public abstract class ServiceUnavailableException : Exception
	{
		protected ServiceUnavailableException(string message)
			: base(message)
		{
		}
	}
}
