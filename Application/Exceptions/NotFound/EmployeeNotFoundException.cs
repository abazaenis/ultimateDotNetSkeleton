namespace UltimateDotNetSkeleton.Application.Exceptions.NotFound
{
	public class EmployeeNotFoundException : NotFoundException
	{
		public EmployeeNotFoundException(Guid employeeId)
			: base($"Employee with id: {employeeId} doesn't exist in the database")
		{
		}
	}
}
