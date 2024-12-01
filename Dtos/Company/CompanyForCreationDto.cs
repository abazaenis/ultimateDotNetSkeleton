namespace UltimateDotNetSkeleton.Dtos.Company
{
	public record CompanyForCreationDto
	{
		public required string Name { get; set; }

		public required string Address { get; set; }

		public required string Country { get; set; }
	}
}
