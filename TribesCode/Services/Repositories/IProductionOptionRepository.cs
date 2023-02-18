using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public interface IProductionOptionRepository
    {
        int GetProductionTimeByProductionOptionId(int productionOptionId);
        List<Building> GetAllAvailableProductionOptions(int playerId, int kingdomId);
        bool CheckIfProductionOptionExistsById(int poId);
        ProductionOption GetProductionOptionById(int prodOptId);
    }
}