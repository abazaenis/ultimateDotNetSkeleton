namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;

	public record AppleUserForRegistrationDto : BaseUserForRegistrationDto
	{
		[Required]
		public string AppleId { get; init; } = string.Empty;

		[Required]
		public string DeviceId { get; init; } = string.Empty;
	}
}
