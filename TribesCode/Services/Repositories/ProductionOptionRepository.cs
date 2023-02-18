using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public class ProductionOptionRepository : IProductionOptionRepository
    {
        private readonly IContext _context;

        public ProductionOptionRepository(IContext context)
        {
            _context = context;
        }

        public int GetProductionTimeByProductionOptionId(int productionOptionId)
        {
            return _context.ProductionOptions
                .FirstOrDefault(po => po.Id == productionOptionId)
                .ProdTime;
        }

        public List<Building> GetAllAvailableProductionOptions(int playerId, int kingdomId)
        {
            return _context.Buildings
                .Include(b => b.BuildingType)
                    .ThenInclude(bt => bt.ProductionOptions)
                        .ThenInclude(po => po.ResourceType)
                .Where(b => b.KingdomId == kingdomId
                        && b.BuildingType.Name != "Townhall" 
                        && b.BuildCompletedAt <= DateTime.Now)
                .ToList();
        }

        public bool CheckIfProductionOptionExistsById(int poId)
        {
            return _context.ProductionOptions
                .Any(po => po.Id == poId);
        }

        public ProductionOption GetProductionOptionById(int prodOptId)
        {
            return _context.ProductionOptions
                .FirstOrDefault(po => po.Id == prodOptId);
        }
    }
}