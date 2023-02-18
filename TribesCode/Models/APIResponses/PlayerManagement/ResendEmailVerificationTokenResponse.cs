using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement
{
    public class ResendEmailVerificationTokenResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public string Instructions { get; } = "To verify, use the link in the latest email in your inbox received from us. Previous tokens are invalidated when a new token is sent! The current token expires in one hour.";
    }
}