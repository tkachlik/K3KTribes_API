namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class BuildingCreatedDTO
    {
        public string BuildingType { get; set; }
        public int BuildingLevel { get; set; }
        public int BuildingId { get; set; }
        public string BelongsToKingdom { get; set; }
        public int KingdomId { get; set; }
    }
}