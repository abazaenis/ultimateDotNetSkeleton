namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public abstract record BaseUserForAuthenticationDto
	{
		[Required]
		[EmailAddress]
		[MaxLength(50)]
		public string Email { get; init; } = string.Empty;

		[Required]
		[MaxLength(255)]
		public string DeviceId { get; init; } = string.Empty;
	}
}
