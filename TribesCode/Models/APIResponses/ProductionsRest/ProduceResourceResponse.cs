using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.ProductionsRest
{
    public class ProduceResourceResponse: IResponse
    {
        public string Status { get; } = "Ok";
        public ProductionCreatedDTO ProductionStarted { get; init; }

        public ProduceResourceResponse() { }

        public ProduceResourceResponse(ProductionCreatedDTO productionStarted)
        {
            ProductionStarted = productionStarted;
        }
    }
}