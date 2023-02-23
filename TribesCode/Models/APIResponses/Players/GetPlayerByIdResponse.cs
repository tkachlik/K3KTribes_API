using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Players
{
    public class GetPlayerByIdResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public PlayerDTO Player { get; init; }

        public GetPlayerByIdResponse() { }

        public GetPlayerByIdResponse(PlayerDTO player)
        {
            Player = player;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as GetPlayerByIdResponse;

            if (other == null) return false;

            if (Status != other.Status || !Player.Equals(other.Player)) return false;

            return true;
        }
    }
}