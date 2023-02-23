using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Kingdoms;

public class GetKingdomResponse : IResponse
{
        public string Status { get; } = "Ok";
        public KingdomStatsDTO Kingdom { get; init; }
    
        public GetKingdomResponse() { }

        public GetKingdomResponse(KingdomStatsDTO kingdom)
        {
                Status = "Ok";
                Kingdom = kingdom;
        }
        
        public override bool Equals(object? obj)
        {
                var other = obj as GetKingdomResponse;

                if (other == null) return false;

                if (Status != other.Status || !Kingdom.Equals(other.Kingdom)) return false;

                return true;
        }
}
