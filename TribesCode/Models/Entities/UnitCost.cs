namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class UnitCost
    {
        public int Id { get; set; }

        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }

        public int ResourceTypeId { get; set; }
        public ResourceType ResourceType { get; set; }

        private int amount;
        public int Amount
        {
            get => amount;
            set => amount = value < 0 ? 0 : value;
        }

        public UnitCost()
        {
        }

        public UnitCost(int unitTypeId, int resourceTypeId)
        {
            UnitTypeId = unitTypeId;
            ResourceTypeId = resourceTypeId;
            Amount = 0;
        }
        //if amount is not added to the constructor then the default value is 0

        public UnitCost(int unitTypeId, int resourceTypeId, int amount)
        {
            UnitTypeId = unitTypeId;
            ResourceTypeId = resourceTypeId;
            Amount = amount;
        }
    }
}