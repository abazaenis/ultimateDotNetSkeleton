namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record AppleUserForAuthenticationDto
	{
		[Required(ErrorMessage = "Email is required")]
		public string? Email { get; init; }

		[Required(ErrorMessage = "Apple ID is required")]
		public string? AppleId { get; init; }

		[Required(ErrorMessage = "Device ID is required")]
		public string? DeviceId { get; init; }
	}
}
