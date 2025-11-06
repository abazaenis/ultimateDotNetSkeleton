namespace UltimateDotNetSkeleton.Application.Exceptions.NotFound
{
	public class NotFoundException : Exception
	{
		protected NotFoundException(string message)
			: base(message)
		{
		}
	}
}
