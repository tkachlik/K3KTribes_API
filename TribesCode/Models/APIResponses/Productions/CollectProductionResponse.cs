using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Productions
{
    public class CollectProductionResponse: IResponse
    {
        public string Status { get; } = "Ok";
        public ProductionCollectedOrDeletedDTO ProductionCollected { get; init; }

        public CollectProductionResponse() { }
        public CollectProductionResponse(ProductionCollectedOrDeletedDTO productionCollected)
        {
            ProductionCollected = productionCollected;
        }
    }
}