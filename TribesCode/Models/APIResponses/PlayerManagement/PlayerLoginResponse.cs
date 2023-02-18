using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement
{
    public class PlayerLoginResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public string AuthenticationToken { get; init; }

        public PlayerLoginResponse() { }

        public PlayerLoginResponse(string authenticationToken)
        {
            AuthenticationToken = authenticationToken;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as PlayerLoginResponse;

            if (other == null) return false;

            if (Status != other.Status || AuthenticationToken == null) return false;
            
            return true;
        }
    }
}