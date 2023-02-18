using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.ProductionsRest
{
    public class DeleteProductionResponse: IResponse
    {
        public string Status { get; } = "Ok";
        public string Details { get; init; }
        public ProductionCollectedOrDeletedDTO ProductionDeleted { get; init; }

        public DeleteProductionResponse() { }

        public DeleteProductionResponse(ProductionCollectedOrDeletedDTO productionCollectedOrDeletedDTO)
        {
            Details = "The Production was cancelled and your Kingdom got back all the Food originally spent on the Production.";
            ProductionDeleted = productionCollectedOrDeletedDTO;
        }

        public DeleteProductionResponse(int foodAmount, ProductionCollectedOrDeletedDTO productionCollectedOrDeletedDTO)
        {
            Details = $"The Production was cancelled, but your Kingdom does not have enough free Food Storage to get back all the Food originally spent on the Production, so you will only receive {foodAmount} Food units in return.";
            ProductionDeleted = productionCollectedOrDeletedDTO;
        }
    }
}