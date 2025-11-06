namespace UltimateDotNetSkeleton.Application.DTOs.Email
{
	using System.Text.Json.Serialization;

	public record EmailRecipientDto
	{
		[JsonPropertyName("email")]
		public string Email { get; set; } = string.Empty;

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
	}
}
