using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.KingdomRest;

public class CreateKingdomResponse : IResponse
{
    public string Status { get; } = "Ok";
    public MyKingdomDTO KingdomCreated { get; init; }

    public CreateKingdomResponse() { }

    public CreateKingdomResponse(MyKingdomDTO kingdomCreated)
    {
        KingdomCreated = kingdomCreated;
    }
}