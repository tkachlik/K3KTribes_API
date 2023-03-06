using dusicyon_midnight_tribes_backend.Models.APIRequests.Buildings;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services;

public interface IBuildingService
{
    public IResponse Create(CreateBuildingRequest request, int playerId);
    public IResponse Upgrade(int buildingId, int playerId);
    public IResponse ShowConstructionOptions(int kingdomId, int playerId);
    public IResponse ShowAvailableUpgrades(int kingdomId, int playerId);
    public IResponse ShowBuildingsUnderConstruction(int kingdomId, int playerId);
}