namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public interface IBuildingTypeRepository
    {
        int GetDefaultBuildingTypeIdByName(string buildingTypeName);
        int GetDefaultMaxStorageMultiplicatorByLowestSeedLevel();
        int GetUpgradeCost(int buildingTypeId);
        public int GetTownhallMaxLevel();
    }
}