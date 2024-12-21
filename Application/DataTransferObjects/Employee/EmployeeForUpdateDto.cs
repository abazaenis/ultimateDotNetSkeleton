namespace UltimateDotNetSkeleton.Application.DataTransferObjects.Employee
{
    public class EmployeeForUpdateDto
    {
        public required string Name { get; set; }

        public int Age { get; set; }

        public required string Position { get; set; }
    }
}
