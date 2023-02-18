using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Domain.GameConfig;
using Microsoft.EntityFrameworkCore;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public class BuildingTypeRepository : IBuildingTypeRepository
    {
        private readonly IContext _context;
        private readonly IGameConfig _gameConfig;
        public BuildingTypeRepository(IContext context,IGameConfig gameConfig)
        {
            _context = context;
            _gameConfig = gameConfig;
        }
        
        public int GetDefaultMaxStorageMultiplicatorByLowestSeedLevel()
        {
            //To seed db context from diffrent level, first we need to get lowest level and then multiplicate maxStroge
            // with this function we are able to seed db from 4 to 18 level for example

            int maxStorageMultiplicator = _context
                .BuildingTypes
                .Min(x => x.Level);

            return maxStorageMultiplicator;
        }

        public int GetDefaultBuildingTypeIdByName(string buildingTypeName)
        {
            // get Id using bulidng type name to get lowest level building type id
            // with this function we are able to seed db from 4 to 18 level for example

            var minlevel = _context.BuildingTypes.Min(x => x.Level);
            var buildingTypeId = _context.BuildingTypes.Where(b => b.Name == buildingTypeName && b.Level == minlevel).FirstOrDefault().Id; 

            return buildingTypeId;
        }

        public int GetUpgradeCost(int buildingTypeId)
        {
            //this checks for max level, If the Id increases but the level decreases then it means
            //that its not the same building anymore and returns 0
            //also doesnt include townhall because it has different upgrade rules
            var isTownhall = _context.BuildingTypes
                    .Any(b => b.Id == buildingTypeId && b.Name == _gameConfig.GetTownhallName());
            if (!isTownhall)
            {
                var buildingPreUpgrade = _context.BuildingTypes
                    .Include(b => b.BuildingCost)
                    .FirstOrDefault(b => b.Id == buildingTypeId && b.Name != _gameConfig.GetTownhallName());

                var buildingUpgrade = _context.BuildingTypes
                    .Include(b => b.BuildingCost)
                    .FirstOrDefault(b => b.Id == buildingTypeId + 1 && b.Name != _gameConfig.GetTownhallName());

                if (buildingPreUpgrade.Level > buildingUpgrade.Level)
                {
                    return 0;
                }

                return buildingUpgrade.BuildingCost.Amount;
            }
            else
            {
                var buildingUpgrade = _context.BuildingTypes
                    .Include(b => b.BuildingCost)
                    .FirstOrDefault(b => b.Id == buildingTypeId + 1);
                if (buildingUpgrade is null)
                {
                    return 0;
                }
                return buildingUpgrade.BuildingCost.Amount;
            }
        }
        
        public int GetTownhallMaxLevel()
        {
            var maxLevel = _gameConfig.GetEndLevelOfSeedData();
            return maxLevel;
        }
    }
}
