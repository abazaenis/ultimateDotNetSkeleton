namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record ChangePasswordDto
	{
		[Required(ErrorMessage = "Old password is required.")]
		public string? Password { get; set; }

		[Required(ErrorMessage = "New password is required.")]
		[MinLength(8, ErrorMessage = "New password must be at least 8 characters.")]
		public string? NewPassword { get; set; }
	}
}
