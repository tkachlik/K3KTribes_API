namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class ProductionOptionDTO
    {
        public string ResourceType { get; set; }
        public int AmountProduced { get; set; }
        public int ProductionTime { get; set; }
        public int ProductionOptionId { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as ProductionOptionDTO;

            if (other == null) return false;

            if (ResourceType != other.ResourceType ||
                AmountProduced != other.AmountProduced ||
                ProductionTime != other.ProductionTime ||
                ProductionOptionId != other.ProductionOptionId) 
                return false;

            return true;
        }
    }
}