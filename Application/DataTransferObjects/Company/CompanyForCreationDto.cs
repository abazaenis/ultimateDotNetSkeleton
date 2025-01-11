namespace UltimateDotNetSkeleton.Application.DataTransferObjects.Company
{
	using System.ComponentModel.DataAnnotations;

	using UltimateDotNetSkeleton.Application.DataTransferObjects.Employee;

	public record CompanyForCreationDto
	{
		[Required(ErrorMessage = "Company name is a required field.")]
		public required string Name { get; set; }

		[Required(ErrorMessage = "Company address is a required field.")]
		public required string Address { get; set; }

		[Required(ErrorMessage = "Company country is a required field.")]
		public required string Country { get; set; }

		public IEnumerable<EmployeeForCreationDto>? Employees { get; set; }
	}
}
