namespace UltimateDotNetSkeleton.Application.DTOs.Email
{
	using System.Text.Json.Serialization;

	public record VerificationEmailDto
	{
		[JsonPropertyName("recipients")]
		public List<RecipientDto> Recipients { get; set; } = [];

		[JsonPropertyName("from")]
		public SenderDto From { get; set; } = new SenderDto();

		[JsonPropertyName("domain")]
		public string Domain { get; set; } = string.Empty;

		[JsonPropertyName("template_id")]
		public string TemplateId { get; set; } = string.Empty;
	}
}
