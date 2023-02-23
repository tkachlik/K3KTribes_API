using dusicyon_midnight_tribes_backend.Models.APIRequests.Kingdoms;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services;

public interface IKingdomService
{
    IResponse GetAllKingdoms();
    IResponse GetKingdomByID(int kingdomId);
    IResponse Create(CreateKingdomRequest request, int playerId);
    IResponse GetAllMyKingdoms(int playerId);
}