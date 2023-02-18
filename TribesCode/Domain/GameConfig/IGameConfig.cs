namespace dusicyon_midnight_tribes_backend.Domain.GameConfig
{
    public interface IGameConfig
    {
        int GetStartLevelOfSeedData();
        int GetEndLevelOfSeedData();
        int GetTimeAccelerator();
        string GetResourcePath();
        int GetMaxStorageStep();
        string GetBuildingTypeName(int indexOfBuildingName);
        public string GetTownhallName();
        List<string> GetAllBuildingTypeNames();
        List<string> GetAllResourcetypeNames();


        void SetStartLevelOfSeedData(int startLevelOfSeedData);
        void SetEndLevelOfSeedData(int endLevelOfSeedData);
        void SetTimeAccelerator(int timeAccelerator);
        void SetResourcePath(string resourcePath);
        int GetMaxAmountFarms();
        int GetMaxAmountMines();

    }
}