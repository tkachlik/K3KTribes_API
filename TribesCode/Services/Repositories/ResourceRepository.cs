using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories;

public class ResourceRepository : IResourceRepository
{
    private readonly IContext _context;
    

    public ResourceRepository(IContext context)
    {
        _context = context;
    }
    public int GetFoodAmount(int kingdomId)
    {
        var food = _context.Resources
            .First(f => f.KingdomId == kingdomId && f.ResourceTypeId == 2);
        
        return food.Amount;
    }
    
    public int GetGoldAmount(int kingdomId)
    {
        var gold = _context.Resources
            .First(f => f.KingdomId == kingdomId && f.ResourceTypeId == 1);
        
        return gold.Amount;
    }
    
    public int CheckPrice(int townhallLevel, string buildingType) // checks how much does it cost to build this building
    {                                                             // based on the Townhall level
        var buildingCostId = _context.BuildingTypes
            .First(b => b.Name.ToLower() == buildingType.ToLower() && b.Level == townhallLevel);

        var price = _context.BuildingCosts
            .First(c => c.Id == buildingCostId.BuildingCostId);

        return price.Amount; // + townhall to add consumption
    }

    public void DecreaseGold(int kingdomId, int amount,int townhallLevel) // used to lower your resources after purchasing something
    {
        var gold = _context.Resources
            .First(f => f.KingdomId == kingdomId && f.ResourceTypeId == 1);
        
        var food = _context.Resources
            .First(f => f.KingdomId == kingdomId && f.ResourceTypeId == 2);

        gold.Amount -= amount;
        food.Amount -= townhallLevel;
    }
    
    public void DecreaseFood(int kingdomId, int amount, int townhallLevel)
    {
        var food = _context.Resources
            .First(f => f.KingdomId == kingdomId && f.ResourceTypeId == 2);

        food.Amount -= amount + townhallLevel;
    }

    public Resource GetFoodByKingdomId(int kingdomId)
    {
        return _context.Resources
            .FirstOrDefault(r => r.KingdomId == kingdomId
                              && r.ResourceTypeId == 2);
    }

    public void UpdateResource(Resource resource)
    {
        _context.Resources
            .Update(resource);
    }

    public Resource GetResourceByKingdomIdAndResourceTypeId(int kingdomId, int resourceTypeId)
    {
        return _context.Resources
            .FirstOrDefault(r => r.KingdomId == kingdomId
                              && r.ResourceTypeId == resourceTypeId);
    }

    public int GetResourceAmount(int kingdomId, int resourceTypeId)
    {
        return _context.Resources
            .FirstOrDefault(r => r.ResourceTypeId == resourceTypeId && r.KingdomId == kingdomId)
            .Amount;
    }
}