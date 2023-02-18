using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Controllers;
using dusicyon_midnight_tribes_backend.Domain.GameConfig;
using dusicyon_midnight_tribes_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace dusicyon_midnight_tribes_backend.Services.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly IContext _context;
    private readonly IGameConfig _gameConfig;
    public BuildingRepository(IContext context, IGameConfig gameConfig)
    {
        _context = context;
        _gameConfig = gameConfig;
    }

    public List<Building> GetAllBuildings()
    {
        var buildings = _context.Buildings
            .Include(k => k.BuildingType)
            .ToList(); // gets all buildings + buildingType
          
        return buildings;
    }
    
    public bool CheckBuildingLimit(int kingdomId,string buildingType)
    {
        if (buildingType.ToLower() == _gameConfig.GetBuildingTypeName(0).ToLower())
        {
            var buildingList = _context.Buildings
                .Include(t => t.BuildingType)
                .Where(b => b.BuildingType.Name == _gameConfig.GetBuildingTypeName(0) && b.KingdomId == kingdomId)
                .ToList(); // gets list of buildings (farms)
            
            if (buildingList.Count >= _gameConfig.GetMaxAmountFarms()) // if the kingdom has 5 farms it wont let you build another one
            {
                return true;
            }
        }
        if (buildingType.ToLower() == _gameConfig.GetBuildingTypeName(1).ToLower())
        {
            var buildingList = _context.Buildings
                .Include(t => t.BuildingType)
                .Where(b => b.BuildingType.Name == _gameConfig.GetBuildingTypeName(1) && b.KingdomId == kingdomId)
                .ToList();
            
            if (buildingList.Count >= _gameConfig.GetMaxAmountMines())
            {
                return true;
            }
        }
        return false;
    }
    
    public bool DoesBuildingExist(int buildingId)
    {
        var building = _context.Buildings
            .FirstOrDefault(b => b.Id == buildingId);
        
        return building == null; // if its null it will return true which will trigger error response
    }

    public Building GetBuildingById(int buildingId)
    {
        var building = _context.Buildings
            .Include(b => b.Productions)
            .Include(b=> b.BuildingType)
                .ThenInclude(bt => bt.BuildingCost)
            .Include(b => b.BuildingType)
                .ThenInclude(bt => bt.ProductionOptions)
            .FirstOrDefault(b => b.Id == buildingId);

        return building;
    }
    
    public int GetKingdomIdFromBuildingId(int buildingId)
    {
        var building = _context.Buildings
            .FirstOrDefault(b => b.Id == buildingId);

        return building.KingdomId; // returns kingdomId from building
    }
    
    public int GetBuildingTime(int buildingTypeId)
    {
        var time = _context.BuildingCosts
            .FirstOrDefault(t => t.BuildingTypeId == buildingTypeId);

        return time.BuildTime;
    }
    
    public int GetUpgradeTime(int buildingTypeId)
    {
        var time = _context.BuildingCosts
            .FirstOrDefault(t => t.BuildingTypeId == buildingTypeId + 1);

        return time.BuildTime;
    }

    public void CreateBuilding(Building building)
    {
        _context.Buildings
            .Add(building);
    }

    public bool CheckLevel(int buildingId)
    {
        var building = GetBuildingById(buildingId);
        var levelCheck = _context.BuildingTypes
            .FirstOrDefault(l => l.Level == building.BuildingType.Level + 1);
        
        return levelCheck == null;
    }

    public void UpgradeBuilding(int buildingId)
    {
        var building = GetBuildingById(buildingId);
        
        building.BuildingTypeId += 1;
        building.BuildStartedAt = DateTime.Now;
        building.BuildCompletedAt = DateTime.Now.Add(TimeSpan.FromSeconds(GetUpgradeTime(building.BuildingTypeId)));
        
        _context.Buildings
            .Update(building);
    }

    public Building GetTownhall(int kingdomId)
    {
        var townhall = _context.Buildings
            .Include(b => b.BuildingType)
            .FirstOrDefault(b => b.BuildingType.Name == _gameConfig.GetTownhallName() && b.KingdomId == kingdomId);

        return townhall;
    }
    public List<Building> GetAllKingdomBuildings(int kingdomId)
    {
        var buildings = _context.Buildings
            .Include(b=> b.BuildingType)
            .ThenInclude(b => b.BuildingCost)
            .Where(b => b.KingdomId == kingdomId)
            .ToList();
        
        return buildings;
    }

    public List<Building> GetAllBuildingsUnderConstruction(int kingdomId)
    {
        var buildings = _context.Buildings
            .Include(b => b.BuildingType)
            .Where(b => b.BuildCompletedAt > DateTime.Now)
            .ToList();

        return buildings;
    }

    public bool CheckIfBuildingIsUnderConstruction(int buildingId)
    {
        // this code assumes that the building exists because it had to go through previous method that 
        // proves its existence
        var building = _context.Buildings
            .FirstOrDefault(b => b.Id == buildingId);
        return building.BuildCompletedAt > DateTime.Now;
    }

    public int CheckTownHallLevel(int kingdomId) 
    {
        var townhall =  //find the townhall based on the kingdomId and buildingTypeId
             GetAllBuildings()
            .FirstOrDefault(b => b.KingdomId == kingdomId && b.BuildingType.Name == _gameConfig.GetTownhallName());
        // its FirstOrDefault because there is only one kingdom with that specific id anyway
        if (townhall == null)
        {
            return 0;
        }
        return townhall.BuildingType.Level; // if it returns null it means that the kingdom doesnt exist because townhalls are created instantly
        // when new kingdom is made
    }
    
    public int GetBuildingTypeId(string buildingType, int kingdomId) // gets you buildingTypeId based on townhalls level
    {
        if (buildingType.ToLower() == _gameConfig.GetBuildingTypeName(0).ToLower())
        {
            var buildingTypeId = _context.BuildingTypes.FirstOrDefault(b =>
                b.Name == _gameConfig.GetBuildingTypeName(0) && b.Level == CheckTownHallLevel(kingdomId));

            return buildingTypeId.Id;
        }
        else
        {
            var buildingTypeId = _context.BuildingTypes.FirstOrDefault(b =>
                b.Name == _gameConfig.GetBuildingTypeName(1) && b.Level == CheckTownHallLevel(kingdomId));

            return buildingTypeId.Id;
        }
    }

}