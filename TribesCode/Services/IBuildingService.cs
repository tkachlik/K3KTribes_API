using dusicyon_midnight_tribes_backend.Models.APIRequests.BuildingRest;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services;

public interface IBuildingService
{
    public IResponse Create(CreateBuildingRequest request, int playerId);
    public IResponse Upgrade(UpgradeBuildingRequest request, int playerId);
    public IResponse ConstructionOptions(ConstructionOptionsRequest request, int playerId);
    public IResponse ShowAvailableUpgrades(ShowAvailableUpgradesRequest request, int playerId);
    public IResponse ShowBuildingsUnderConstruction(ShowBuildingsUnderConstructionRequest request, int playerId);
}