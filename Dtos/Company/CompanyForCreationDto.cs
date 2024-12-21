namespace UltimateDotNetSkeleton.Dtos.Company
{
	using UltimateDotNetSkeleton.Dtos.Employee;

	public record CompanyForCreationDto
	{
		public required string Name { get; set; }

		public required string Address { get; set; }

		public required string Country { get; set; }

		public IEnumerable<EmployeeForCreationDto>? Employees { get; set; }
	}
}
