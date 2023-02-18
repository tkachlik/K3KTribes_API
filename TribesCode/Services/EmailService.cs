using SendGrid;
using SendGrid.Helpers.Mail;
using static System.Net.WebRequestMethods;

namespace dusicyon_midnight_tribes_backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly SendGridClient _client;
        private readonly string _verifyEmailLink;
        private readonly string _passwordResetLink;

        public EmailService(IConfiguration config)
        {
            _config = config;
            _client = new SendGridClient(_config["EmailSender:SENDGRID_API_KEY"]);
            
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                _verifyEmailLink = "https://tribesk3game.azurewebsites.net/api/email/verify/";
                _passwordResetLink = "https://tribesk3game.azurewebsites.net/api/playermanagement/reset-password/";
            }
            else
            {
                _verifyEmailLink = "https://localhost:7038/api/email/verify/";
                _passwordResetLink = "https://localhost:7038/api/playermanagement/reset-password/";
            }
        }

        // PUBLIC INTERFACE METHODS
        public async Task SendEmailWithEmailVerificationToken(string token, string recipientAddress, string recipientName)
        {
            var email = GenerateEmailWithVerificationToken(token);
            email.AddTo(recipientAddress, recipientName);
            var result = await _client.SendEmailAsync(email).ConfigureAwait(false);
        }

        public async Task SendEmailWithPasswordResetToken(string token, string recipientAddress, string recipientName)
        {
            var email = GenerateEmailWithPasswordResetToken(token);
            email.AddTo(recipientAddress, recipientName);
            var result = await _client.SendEmailAsync(email).ConfigureAwait(false);
        }

        // PRIVATE HELPER METHODS
        private SendGridMessage GenerateEmailWithVerificationToken(string token)
        {
            string content = $"<h1>Verify your email by clicking the link below.</h1>" +
                $"<h3>Click <a href=\"{_verifyEmailLink}{token}\">here</a>.</h3>" +
                "<p>If you made multiple email verification requests, only use the link in the latest email you received - when a new email verification request is made, all previous tokens are invalidated.<p>" +
                $"<p>The verification token expires in one hour.</p>";

            var email = new SendGridMessage()
            {
                From = new EmailAddress("k3t.ps.noreply@gmail.com", "K3 Tribes - Player Service"),
                Subject = "Verify your email",
                HtmlContent = content
            };

            return email;
        }

        private SendGridMessage GenerateEmailWithPasswordResetToken(string token)
        {
            string content = $"<h1>Reset your password by clicking the link below.</h1>" +
                $"<h3>Click <a href=\"{_passwordResetLink}{token}\">here</a>.</h3>" +
                $"<p>If you made multiple password reset requests, only use the link in the latest email you received - when a new password reset request is made, all previous tokens are invalidated.<p>" +
                $"<p>The password reset token expires in one hour.</p>";

            var email = new SendGridMessage()
            {
                From = new EmailAddress("k3t.ps.noreply@gmail.com", "K3 Tribes - Player Service"),
                Subject = "Password reset (for K3 Tribes)",
                HtmlContent = content
            };

            return email;
        }
    }
}