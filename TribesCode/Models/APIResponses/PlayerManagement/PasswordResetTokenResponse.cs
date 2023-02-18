using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement
{
    public class PasswordResetTokenResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public string Instructions { get; } = "Go to your email inbox and click the link in the email. --- If you have made multiple requests for password reset, only use the latest link in your email to reset password, as previous tokens are invalidated. --- Password reset tokens expire in one hour.";
    }
}