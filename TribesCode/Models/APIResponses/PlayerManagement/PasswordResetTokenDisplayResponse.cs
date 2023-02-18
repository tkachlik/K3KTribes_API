using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement
{
    public class PasswordResetTokenDisplayResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public string Token { get; init;  }

        public PasswordResetTokenDisplayResponse() { }

        public PasswordResetTokenDisplayResponse(string token)
        {
            Token = token;
        }
    }
}
