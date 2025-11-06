namespace UltimateDotNetSkeleton.Application.ConfigurationModels
{
	public class EmailConfiguration
	{
		public const string Section = "Msg91:email";

		public string? AuthKey { get; set; }

		public string? Url { get; set; }

		public string? Domain { get; set; }

		public string? SenderEmail { get; set; }

		public string? VerificationTemplateId { get; set; }
	}
}
