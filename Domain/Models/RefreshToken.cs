namespace UltimateDotNetSkeleton.Domain.Models
{
	public class RefreshToken
	{
		public Guid Id { get; set; }

		public string? Token { get; set; }

		public DateTime ExpirationDate { get; set; }

		public Guid UserId { get; set; }

		public string? DeviceId { get; set; }

		// Navigational properties
		public User? User { get; set; }
	}
}
