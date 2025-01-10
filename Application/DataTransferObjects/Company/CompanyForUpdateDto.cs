namespace UltimateDotNetSkeleton.Application.DataTransferObjects.Company
{
	using UltimateDotNetSkeleton.Application.DataTransferObjects.Employee;

	public class CompanyForUpdateDto
	{
        public required string Name { get; set; }

        public required string Address { get; set; }

        public required string Country { get; set; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
    }
}
