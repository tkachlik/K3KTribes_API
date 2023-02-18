namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class ProductionCreatedDTO
    {
        public string ResourceType { get; set; }
        public int AmountProduced { get; set; }
        public string WillBeReadyAt { get; set; }
        public int ProductionId { get; set; }
    }
}