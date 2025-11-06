namespace UltimateDotNetSkeleton.Application.DTOs.Email
{
	using System.Text.Json.Serialization;

	public record RecipientDto
	{
		[JsonPropertyName("to")]
		public List<EmailRecipientDto> To { get; set; } = [];

		[JsonPropertyName("variables")]
		public Dictionary<string, string> Variables { get; set; } = [];
	}
}
