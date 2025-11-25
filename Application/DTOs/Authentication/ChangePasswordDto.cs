namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record ChangePasswordDto
	{
		[Required]
		public string Password { get; set; } = string.Empty;

		[Required]
		[Length(8, 50)]
		public string NewPassword { get; set; } = string.Empty;
	}
}
