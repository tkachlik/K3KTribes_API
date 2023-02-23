using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Productions
{
    public class ShowAvailableProductionOptionsResponse : IResponse
    {
        public string Status { get; } = "Ok";
        public string Instructions { get; } = "Production in any Building will cost an amount of food equal to the building level (e.g. farm level 3 production will cost you 3 food units).";
        public List<ProductionOptionsAvailableOnCompletedBuildingsDTO> ProductionOptionsAvailableOnCompletedBuildings { get; init; }

        public ShowAvailableProductionOptionsResponse() { }

        public ShowAvailableProductionOptionsResponse(List<ProductionOptionsAvailableOnCompletedBuildingsDTO> productionOptionsAvailableOnCompletedBuildings)
        {
            ProductionOptionsAvailableOnCompletedBuildings = productionOptionsAvailableOnCompletedBuildings;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ShowAvailableProductionOptionsResponse;

            if (other == null) return false;

            if (Status != other.Status || 
                Instructions != other.Instructions || 
                ProductionOptionsAvailableOnCompletedBuildings.Count != other.ProductionOptionsAvailableOnCompletedBuildings.Count) 
                return false;

            for (int i = 0; i < ProductionOptionsAvailableOnCompletedBuildings.Count; i++)
            {
                if (!ProductionOptionsAvailableOnCompletedBuildings[i]
                    .Equals(other.ProductionOptionsAvailableOnCompletedBuildings[i])) return false;
            }

            return true;
        }
    }
}