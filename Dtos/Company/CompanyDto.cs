namespace UltimateDotNetSkeleton.Dtos.Company
{
	public record CompanyDto
	{
		public Guid Id { get; set; }

		public required string Name { get; set; }

		public required string FullAddress { get; set; }
	}
}
