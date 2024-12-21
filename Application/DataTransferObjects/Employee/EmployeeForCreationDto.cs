namespace UltimateDotNetSkeleton.Application.DataTransferObjects.Employee
{
    public record EmployeeForCreationDto
    {
        public required string Name { get; set; }

        public required int Age { get; set; }

        public required string Position { get; set; }
    }
}
