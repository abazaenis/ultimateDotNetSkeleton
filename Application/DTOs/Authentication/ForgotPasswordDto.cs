namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record ForgotPasswordDto
	{
		[Required]
		[EmailAddress]
		[MaxLength(50)]
		public string Email { get; set; } = string.Empty;
	}
}
