namespace UltimateDotNetSkeleton.Infrastructure.Services.EmailSender
{
    public interface IEmailSender
	{
		Task SendVerificationCodeAsync(string recipient, int verificationCode);

		Task SendTemporaryPasswordAsync(string recipient, string temporaryPassword);
	}
}
