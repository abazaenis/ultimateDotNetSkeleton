namespace UltimateDotNetSkeleton.Domain.Models
{
	using System.ComponentModel.DataAnnotations.Schema;
	using UltimateDotNetSkeleton.Domain.Enums;

	public class User
	{
		public Guid Id { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public string? PhoneNumber { get; set; }

		public string? Email { get; set; }

		public bool EmailNotifications { get; set; } = true;

		public string? PasswordHash { get; set; }

		public string? PasswordSalt { get; set; }

		public string? ThirdPartyId { get; set; }

		public string? TempPasswordHash { get; set; }

		public string? TempPasswordSalt { get; set; }

		public DateTime? TempPasswordExpiry { get; set; }

		public RegistrationType RegistrationType { get; set; }

		public DateTime RegistrationDate { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? DeletedAt { get; set; }

		[InverseProperty("User")]
		public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
	}
}
