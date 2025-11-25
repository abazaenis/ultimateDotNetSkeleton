namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record ChangePasswordWithTemporaryDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string TemporaryPassword { get; set; } = string.Empty;

		[Required]
		[MinLength(8)]
		public string NewPassword { get; set; } = string.Empty;
	}
}
