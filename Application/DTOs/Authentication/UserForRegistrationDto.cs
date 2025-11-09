namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record UserForRegistrationDto : BaseUserForRegistrationDto
	{
		[Required]
		[Length(8, 50)]
		public string Password { get; init; } = string.Empty;
	}
}
