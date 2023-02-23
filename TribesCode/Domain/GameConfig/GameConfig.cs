using System.Runtime.InteropServices;

namespace dusicyon_midnight_tribes_backend.Domain.GameConfig
{
    public class GameConfig : IGameConfig
    {

        private int _startLevelOfSeedData = 1;
        private int _endLevelOfSeedData = 20;
        private int _timeAccelerator = 30;
        private string _resourcePath=@"Domain\SeedData\";

        private int _maxAmountOfFarms = 5;
        private int _maxAmountOfMines = 3;

        private readonly int _maxStorageStep = 1200;
        
        private readonly List<string> _buildingTypeNames = new List<string>() //order MATTERS
        {
            "Farm",     //0
            "Mine",     //1
                        //2
                        //3
                        //4
                        //...

            "Townhall"  // Townhall must stay always last in case of update this list 
        };

        private readonly List<string> _resourceTypeNames = new List<string>() //order MATTERS
        {
            "Gold",     //0
            "Food"      //1
                        //2
                        //3
                        //4
                        //...
        };

        
        public int GetStartLevelOfSeedData() { return _startLevelOfSeedData; }
        public int GetEndLevelOfSeedData() { return _endLevelOfSeedData; }
        public int GetTimeAccelerator() { return _timeAccelerator; }
        public string GetResourcePath() { return _resourcePath; }
        public int GetMaxStorageStep() { return _maxStorageStep; }
        public string GetBuildingTypeName(int indexOfBuildingName) { return _buildingTypeNames[indexOfBuildingName]; }
        public string GetTownhallName() { return _buildingTypeNames.Last(); }
        public List<string> GetAllBuildingTypeNames() { return _buildingTypeNames; }
        public List<string> GetAllResourcetypeNames() { return _resourceTypeNames; }

        public int GetMaxAmountFarms() {return _maxAmountOfFarms;}
        public int GetMaxAmountMines() {return _maxAmountOfMines;}
        public void SetStartLevelOfSeedData(int startLevelOfSeedData) { _startLevelOfSeedData = startLevelOfSeedData; }
        public void SetEndLevelOfSeedData(int endLevelOfSeedData) { _endLevelOfSeedData = endLevelOfSeedData; }
        public void SetTimeAccelerator(int timeAccelerator) { _timeAccelerator = timeAccelerator; }
        public void SetResourcePath(string resourcePath) { _resourcePath = resourcePath; }
        
        public string CheckAndReturnPathBasedOnOS(string resourcePath)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                _resourcePath = _resourcePath.Replace("\\", "/");
                return _resourcePath;
            }
            return _resourcePath;
        }
    }
}
