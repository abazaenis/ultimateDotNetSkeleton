namespace UltimateDotNetSkeleton.Domain.ConfigurationModels
{
	public class JwtConfiguration
	{
		public string Section { get; set; } = "JwtSettings";

		public string? ValidIssuer { get; set; }

		public string? ValidAudience { get; set; }

		public int Expires { get; set; }

		public string? SecretKey { get; set; }
	}
}
