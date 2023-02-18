namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class ProductionOption
    {
        public int Id { get; set; }
        
        private int amount;
        public int Amount
        {
            get => amount;
            set => amount = value < 0 ? 0 : value;
        }

        public int BuildingTypeId { get; set; }
        public BuildingType BuildingType{ get; set; }
        
        public int? ResourceTypeId { get; set; }
        public ResourceType? ResourceType { get; set; }
        
        public int? UnitTypeId { get; set; }
        public UnitType? UnitType { get; set; }

        public int ProdTime { get; set; }

        public List<Production> Productions { get; set; }


        public ProductionOption() { }

        public ProductionOption(int id, int buildingTypeId, int? resourceTypeId, int? unitTypeId, int amount, int prodTime)
        {
            Id = id;
            BuildingTypeId = buildingTypeId;
            ResourceTypeId = resourceTypeId;
            UnitTypeId = unitTypeId;
            Amount = amount;
            ProdTime = prodTime;
            Productions = new List<Production>();
        }
    }
}