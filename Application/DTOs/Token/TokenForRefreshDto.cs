namespace UltimateDotNetSkeleton.Application.DTOs.Token
{
	using System.ComponentModel.DataAnnotations;

	public record TokenForRefreshDto
	{
		[Required(ErrorMessage = "Access token is required")]
		public string? AccessToken { get; set; }

		[Required(ErrorMessage = "Refresh token is required")]
		public string? RefreshToken { get; set; }

		[Required(ErrorMessage = "Device ID is required")]
		public string? DeviceId { get; set; }
	}
}
