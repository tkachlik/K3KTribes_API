using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public interface IProductionRepository
    {
        void AddProduction(Production production);
        List<Production> GetAllUncollectedProductions(int kingdomId);
        Production GetProductionById(int productionId);
        void UpdateProduction(Production production);
        void DeleteProduction(Production production);
    }
}