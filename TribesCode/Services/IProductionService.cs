using dusicyon_midnight_tribes_backend.Models.APIRequests.Productions;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services
{
    public interface IProductionService
    {
        IResponse ProduceResource(int playerId, ProduceResourceRequest request);
        IResponse ShowUncollectedProductions(int playerId, int kingdomId);
        IResponse CollectProduction(int playerId, int productionId);
        IResponse DeleteProduction(int playerId, int productionId);
    }
}