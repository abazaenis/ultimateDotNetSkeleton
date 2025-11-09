namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record GoogleUserForAuthenticationDto
	{
		[Required(ErrorMessage = "Email is required")]
		public string? Email { get; init; }

		[Required(ErrorMessage = "Google ID is required")]
		public string? GoogleId { get; init; }

		[Required(ErrorMessage = "Device ID is required")]
		public string? DeviceId { get; init; }
	}
}
