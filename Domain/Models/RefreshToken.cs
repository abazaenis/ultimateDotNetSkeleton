namespace UltimateDotNetSkeleton.Domain.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("RefreshTokens")]
	public class RefreshToken
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("RefreshTokenId")]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Token { get; set; } = string.Empty;

		[Required]
		[Column(TypeName = "timestamp without time zone")]
		public DateTime ExpirationDate { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[Required]
		[Length(5, 255)]
		public string DeviceId { get; set; } = string.Empty;

		// Navigational properties
		public User? User { get; set; }
	}
}