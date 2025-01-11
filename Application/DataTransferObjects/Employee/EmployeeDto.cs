namespace UltimateDotNetSkeleton.Application.DataTransferObjects.Employee
{
	public record EmployeeDto
	{
		public Guid Id { get; set; }

		public required string Name { get; set; }

		public required int Age { get; set; }

		public required string Position { get; set; }
	}
}
