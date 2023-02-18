using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories;

public interface IResourceRepository
{
    public int GetGoldAmount(int kingdomId);
    public int GetFoodAmount(int kingdomId);
    public int CheckPrice(int townhallLevel, string buildingType);
    public void DecreaseGold(int kingdomId, int amount, int townhallLevel);
    public void DecreaseFood(int kingdomId, int amount, int townhallLevel);
    Resource GetFoodByKingdomId(int kingdomId);
    void UpdateResource(Resource resource);
    Resource GetResourceByKingdomIdAndResourceTypeId(int kingdomId, int resourceTypeId);
    int GetResourceAmount(int kingdomId, int resourceTypeId);
}