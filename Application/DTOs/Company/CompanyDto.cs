namespace UltimateDotNetSkeleton.Application.DTOs.Company
{
    public record CompanyDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string FullAddress { get; set; }
    }
}
