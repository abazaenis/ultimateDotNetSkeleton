namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record UserForAuthenticationDto : BaseUserForAuthenticationDto
	{
		[Required]
		[MaxLength(50)]
		public string Password { get; init; } = string.Empty;
	}
}
