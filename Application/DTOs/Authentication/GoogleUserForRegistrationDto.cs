namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
	using System.ComponentModel.DataAnnotations;
	using System.Text.Json.Serialization;

	using UltimateDotNetSkeleton.Presentation.Converters;

	public record GoogleUserForRegistrationDto
	{
		[Required(ErrorMessage = "First name is required")]
		[MaxLength(50, ErrorMessage = "Maximum length for first name is 50 characters")]
		public string? FirstName { get; init; }

		[Required(ErrorMessage = "Last name is required")]
		[MaxLength(50, ErrorMessage = "Maximum length for last name is 50 characters")]
		public string? LastName { get; init; }

		[Required(ErrorMessage = "Email is required")]
		[MaxLength(254, ErrorMessage = "Maximum length for email is 254 characters")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		public string? Email { get; init; }

		[Required(ErrorMessage = "Google ID is required")]
		public string? GoogleId { get; init; }

		[Required(ErrorMessage = "Device ID is required")]
		public string? DeviceId { get; init; }

		[Required(ErrorMessage = "Phone number is required")]
		[MaxLength(20, ErrorMessage = "Maximum length for phone number is 20 characters")]
		[JsonConverter(typeof(PhoneNumberConverter))]
		public string? PhoneNumber { get; init; }
	}
}
