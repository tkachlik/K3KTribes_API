using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement
{
    public class EmailVerificationResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public string Instructions { get; } = "Go to your email inbox and click the link in the email. The verification token expires in one hour.";
    }
}