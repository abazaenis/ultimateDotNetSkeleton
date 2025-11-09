namespace UltimateDotNetSkeleton.Application.Exceptions.Conflict
{
	public sealed class UserExistsConflictException : ConflictException
	{
		public UserExistsConflictException(string message)
			: base(message)
		{
		}
	}
}
