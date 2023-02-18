using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories;

public interface IBuildingRepository
{
    public List<Building> GetAllBuildings();
    // public int GetBuildingTypeId(string buildingType, int kingdomId);
    public bool CheckBuildingLimit(int kingdomId, string buildingType);
    public void CreateBuilding(Building building);
    public int GetBuildingTime(int buildingTypeId);
    public int GetKingdomIdFromBuildingId(int buildingId);
    public bool DoesBuildingExist(int buildingId);
    public Building GetBuildingById(int buildingId);
    public bool CheckLevel(int buildingId);
    public void UpgradeBuilding(int buildingId);
    public List<Building> GetAllKingdomBuildings(int kingdomId);
    public int GetUpgradeTime(int buildingTypeId);
    public List<Building> GetAllBuildingsUnderConstruction(int kingdomId);
    public bool CheckIfBuildingIsUnderConstruction(int buildingId);
    public int CheckTownHallLevel(int kingdomId);
    public int GetBuildingTypeId(string buildingType, int kingdomId);
    public Building GetTownhall(int kingdomId);

}