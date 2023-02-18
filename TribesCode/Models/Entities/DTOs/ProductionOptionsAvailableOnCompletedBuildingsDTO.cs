namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class ProductionOptionsAvailableOnCompletedBuildingsDTO
    {
        public string BuildingType { get; set; }
        public int BuildingLevel { get; set; }
        public int BuildingId { get; set; }
        public List<ProductionOptionDTO> ProductionOptions { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as ProductionOptionsAvailableOnCompletedBuildingsDTO;

            if (other == null) return false;

            if (BuildingType != other.BuildingType ||
                BuildingLevel != other.BuildingLevel ||
                BuildingId != other.BuildingId ||
                ProductionOptions.Count != other.ProductionOptions.Count)
                return false;

            for (int i = 0; i < ProductionOptions.Count; i++)
            {
                if (!ProductionOptions[i].Equals(other.ProductionOptions[i])) return false;
            }
            
            return true;
        }
    }
}