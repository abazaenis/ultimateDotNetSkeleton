namespace UltimateDotNetSkeleton.Application.DTOs.Employee
{
    using System.ComponentModel.DataAnnotations;

    public abstract record EmployeeForManipulationDto
    {
		[Required(ErrorMessage = "Employee name is a required field.")]
		[MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
		public required string Name { get; set; }

		[Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
		public int Age { get; set; }

		[Required(ErrorMessage = "Position is a required field.")]
		[MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
		public required string Position { get; set; }
    }
}
