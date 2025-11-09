namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record UserForAuthenticationDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; init; } = string.Empty;

		[Required]
		[Length(8, 30)]
		public string Password { get; init; } = string.Empty;

		[Required]
		[Length(5, 30)]
		public string DeviceId { get; init; } = string.Empty;
	}
}
