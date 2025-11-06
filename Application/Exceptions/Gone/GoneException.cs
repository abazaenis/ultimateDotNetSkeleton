namespace UltimateDotNetSkeleton.Application.Exceptions.Gone
{
	public abstract class GoneException : Exception
	{
		protected GoneException(string message)
			: base(message)
		{
		}
	}
}
