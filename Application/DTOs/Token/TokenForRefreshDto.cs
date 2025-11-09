namespace UltimateDotNetSkeleton.Application.DTOs.Token
{
	using System.ComponentModel.DataAnnotations;

	public record TokenForRefreshDto
	{
		[Required]
		public string AccessToken { get; set; } = string.Empty;

		[Required]
		public string? RefreshToken { get; set; } = string.Empty;

		[Required]
		public string? DeviceId { get; set; } = string.Empty;
	}
}
