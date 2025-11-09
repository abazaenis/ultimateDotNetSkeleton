namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record GoogleUserForAuthenticationDto : BaseUserForAuthenticationDto
	{
		[Required]
		[MaxLength(255)]
		public string GoogleId { get; init; } = string.Empty;
	}
}
