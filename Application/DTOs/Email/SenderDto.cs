namespace UltimateDotNetSkeleton.Application.DTOs.Email
{
	using System.Text.Json.Serialization;

	public record SenderDto
	{
		[JsonPropertyName("email")]
		public string Email { get; set; } = string.Empty;
	}
}
