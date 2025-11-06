namespace UltimateDotNetSkeleton.Application.Exceptions.Conflict
{
	public abstract class ConflictException : Exception
	{
		protected ConflictException(string message)
			: base(message)
		{
		}
	}
}
