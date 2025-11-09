namespace UltimateDotNetSkeleton.Application.DTOs.Authentication
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    using UltimateDotNetSkeleton.Presentation.Converters;

    public abstract record BaseUserForRegistrationDto
    {
		[Required]
		[Length(1, 50)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[Length(1, 50)]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[Length(5, 50)]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[Length(5, 20)]
		[JsonConverter(typeof(PhoneNumberConverter))]
		public string PhoneNumber { get; set; } = string.Empty;
    }
}
