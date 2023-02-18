namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class BuildingCost
    {
        public int Id {get;set;}

        private int amount = 0; //this field amount == cost in our GameRules
        public int Amount
        {
            get => amount;
            set => amount = value < 0 ? 0 : value;
        }

        public int BuildingTypeId { get; set; }
        public BuildingType BuildingType { get; set; }
        
        public int BuildTime { get; set; } //Total time in second

        public BuildingCost() { }
        public BuildingCost(int id, int buildingTypeId, int buildTime, int amount)
        {
            Id = id;
            BuildingTypeId = buildingTypeId;
            BuildTime= buildTime;
            Amount = amount;
        }
    }
}