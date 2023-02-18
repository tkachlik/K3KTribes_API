using dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerRest;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.WorldRest
{
    public class GetWorldByIdResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public WorldDTO World { get; init; }

        public override bool Equals(object? obj)
        {
            var other = obj as GetWorldByIdResponse;

            if (other == null) return false;

            if (Status != other.Status || !World.Equals(other.World)) return false;

            return true;
        }
    }
}
