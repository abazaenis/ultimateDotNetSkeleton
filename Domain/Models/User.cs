namespace UltimateDotNetSkeleton.Domain.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	using UltimateDotNetSkeleton.Domain.Enums;

	[Table("Users")]
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("UserId")]
		public Guid Id { get; set; }

		[Required]
		[Length(1, 50)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[Length(1, 50)]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[Length(5, 20)]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required]
		[Length(5, 50)]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		public bool EmailNotifications { get; set; } = true;

		[Length(1, 255)]
		public string? PasswordHash { get; set; }

		[Length(20, 255)]
		public string? PasswordSalt { get; set; }

		[Length(1, 255)]
		public string? ThirdPartyId { get; set; }

		[Length(1, 255)]
		public string? TempPasswordHash { get; set; }

		[Length(1, 255)]
		public string? TempPasswordSalt { get; set; }

		[Column(TypeName = "timestamp without time zone")]
		public DateTime? TempPasswordExpiry { get; set; }

		[Required]
		[MaxLength(10)]
		public RegistrationType RegistrationType { get; set; }

		[Required]
		[Column(TypeName = "timestamp without time zone")]
		public DateTime RegistrationDate { get; set; }

		public bool IsDeleted { get; set; }

		[Column(TypeName = "timestamp without time zone")]
		public DateTime? DeletedAt { get; set; }

		// Navigational properties
		public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
	}
}