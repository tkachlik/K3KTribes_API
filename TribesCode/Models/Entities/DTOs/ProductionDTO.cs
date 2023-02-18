namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class ProductionDTO
    {
        public string ResourceType { get; set; }
        public int AmountProduced { get; set; }
        public bool IsReadyForCollection { get; set; }
        public string ReadyAt { get; set; }
        public int ProductionId { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as ProductionDTO;

            if (other == null) return false;

            if (ResourceType != other.ResourceType ||
                AmountProduced != other.AmountProduced ||
                IsReadyForCollection != other.IsReadyForCollection ||
                ReadyAt != other.ReadyAt ||
                ProductionId != other.ProductionId)
                return false;

            return true;
        }
    }
}