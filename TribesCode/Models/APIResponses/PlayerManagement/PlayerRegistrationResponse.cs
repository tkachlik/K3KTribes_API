using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement
{
    public class PlayerRegistrationResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public PlayerCreatedDTO PlayerCreated { get; init; }
        public string Recommendation { get; } = "You may now proceed to verify your e-mail address.";

        public PlayerRegistrationResponse() { }

        public PlayerRegistrationResponse(PlayerCreatedDTO playerProfileCreated)
        {
            PlayerCreated = playerProfileCreated;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as PlayerRegistrationResponse;

            if (other == null) return false;

            if (Status != other.Status || 
                Recommendation != other.Recommendation || 
                !PlayerCreated.Equals(other.PlayerCreated)) 
                return false;

            return true;
        }
    }
}