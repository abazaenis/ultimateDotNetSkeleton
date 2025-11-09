namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record AppleUserForAuthenticationDto : BaseUserForAuthenticationDto
	{
		[Required]
		[MaxLength(255)]
		public string AppleId { get; init; } = string.Empty;
	}
}
