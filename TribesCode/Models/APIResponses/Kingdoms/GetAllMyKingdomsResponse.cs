using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Kingdoms;

public class GetAllMyKingdomsResponse: IResponse
{
    public string Status { get; } = "Ok";
    public List<MyKingdomDTO> MyKingdoms { get; init; }

    public GetAllMyKingdomsResponse() { }

    public GetAllMyKingdomsResponse(List<MyKingdomDTO> myKingdoms)
    {
        MyKingdoms = myKingdoms;
    }
    
    public override bool Equals(object? obj)
    {
        var other = obj as GetAllMyKingdomsResponse;

        if (other == null) return false;

        if (Status != other.Status || MyKingdoms.Count != other.MyKingdoms.Count) return false;

        for (int i = 0; i < MyKingdoms.Count; i++)
        {
            if (!MyKingdoms[i].Equals(other.MyKingdoms[i])) return false;
        }
        return true;
    }
}