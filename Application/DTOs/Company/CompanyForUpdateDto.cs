namespace UltimateDotNetSkeleton.Application.DTOs.Company
{
    using UltimateDotNetSkeleton.Application.DTOs.Employee;

    public record CompanyForUpdateDto
    {
        public required string Name { get; set; }

        public required string Address { get; set; }

        public required string Country { get; set; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
    }
}
