namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record ChangePasswordWithTemporaryDto
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Temporary password is required")]
		public string? TemporaryPassword { get; set; }

		[Required(ErrorMessage = "New password is required")]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
		public string? NewPassword { get; set; }
	}
}
