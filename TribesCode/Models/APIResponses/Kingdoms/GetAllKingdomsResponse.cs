using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Kingdoms;

public class GetAllKingdomsResponse : IResponse
{
    public string Status { get; } = "Ok";
    public List<KingdomDTO> KingdomDTOs { get; init; }

    public GetAllKingdomsResponse(){}

    public GetAllKingdomsResponse(List<KingdomDTO> kingdomDtos)
    {
        KingdomDTOs = kingdomDtos;
    }
    
    public override bool Equals(object? obj)
    {
        var other = obj as GetAllKingdomsResponse;

        if (other == null) return false;

        if (Status != other.Status || KingdomDTOs.Count != other.KingdomDTOs.Count) return false;

        for (int i = 0; i < KingdomDTOs.Count; i++)
        {
            if (!KingdomDTOs[i].Equals(other.KingdomDTOs[i])) return false;
        }
        return true;
    }
}