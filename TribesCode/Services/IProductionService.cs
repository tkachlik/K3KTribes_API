using dusicyon_midnight_tribes_backend.Models.APIRequests.Productions;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services
{
    public interface IProductionService
    {
        IResponse ProduceResource(int playerId, ProduceResourceRequest request);
        IResponse ShowAllUncollectedProductions(int playerId, ShowAllUncollectedProductionsRequest request);
        IResponse CollectProduction(int playerId, CollectProductionRequest request);
        IResponse DeleteProduction(int playerId, DeleteProductionRequest request);
    }
}