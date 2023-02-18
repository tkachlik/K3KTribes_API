using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public class ProductionRepository : IProductionRepository
    {
        private readonly IContext _context;

        public ProductionRepository(IContext context)
        {
            _context = context;
        }

        public void AddProduction(Production production)
        {
            _context.Productions
                .Add(production);
        }

        public List<Production> GetAllUncollectedProductions(int kingdomId)
        {
            return _context.Productions
                           .Include(p => p.Building)
                           .Include(p => p.ProductionOption)
                               .ThenInclude(po => po.ResourceType)
                           .Where(p => p.Building.KingdomId == kingdomId 
                                    && p.Collected == false)
                           .ToList();
        }

        public Production GetProductionById (int productionId)
        {
            return _context.Productions
                .Include(p => p.ProductionOption)
                    .ThenInclude(po => po.ResourceType)
                .Include(p => p.Kingdom)
                .Include(p => p.Building)
                    .ThenInclude(b => b.BuildingType)
                .FirstOrDefault(p => p.Id == productionId);
        }

        public void UpdateProduction (Production production)
        {
            _context.Productions
                .Update(production);
        }

        public void DeleteProduction (Production production)
        {
            _context.Productions
                .Remove(production);
        }
    }
}