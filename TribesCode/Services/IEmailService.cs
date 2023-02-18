namespace dusicyon_midnight_tribes_backend.Services
{
    public interface IEmailService
    {
        Task SendEmailWithEmailVerificationToken(string token, string recipientAddress, string recipientName);
        Task SendEmailWithPasswordResetToken(string token, string recipientAddress, string recipientName);
    }
}