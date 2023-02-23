using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Players
{
    public class GetAllPlayersResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public List<PlayerDTO> Players { get; init; }

        public GetAllPlayersResponse() { }
        public GetAllPlayersResponse(List<PlayerDTO> players)
        {
            Players = players;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as GetAllPlayersResponse;

            if (other == null) return false;

            if (Status != other.Status || Players.Count != other.Players.Count) return false;

            for (int i = 0; i < Players.Count; i++)
            {
                if (!Players[i].Equals(other.Players[i])) return false;
            }

            return true;
        }
    }
}