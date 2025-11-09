namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
	public sealed class InvalidRoleException : BadRequestException
	{
		public InvalidRoleException(IEnumerable<string> invalidRoles)
			: base($"The following roles are invalid: {string.Join(", ", invalidRoles)}")
		{
		}
	}
}
