namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class Production
    {
        public int Id { get; set; }

        public bool Collected { get; set; } = false; //default = false
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }

        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }

        public int BuildingId { get; set; }
        public Building Building { get; set; }

        public int ProductionOptionID { get; set; }
        public ProductionOption ProductionOption { get; set; }

        public Production() { }
      
        public Production(int kingdomId, int buildingId, int productionOptionId)
        {
            KingdomId = kingdomId;
            BuildingId = buildingId;
            ProductionOptionID = productionOptionId;
            StartedAt = DateTime.Now;
        }
    }
}