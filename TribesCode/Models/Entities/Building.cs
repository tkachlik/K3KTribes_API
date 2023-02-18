namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class Building
    {
        public int Id { get; set; }

        public int BuildingTypeId { get; set; }
        public BuildingType BuildingType { get; set; }

        public DateTime BuildStartedAt { get; set; }
        public DateTime BuildCompletedAt { get; set; }

        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }

        public List<Production> Productions { get; set; }


        public Building() { }
        public Building(int buildingTypeId)
        {
            BuildingTypeId = buildingTypeId;
            BuildStartedAt= DateTime.Now;
            BuildCompletedAt = BuildStartedAt.AddSeconds(BuildingType.BuildingCost.BuildTime);
            Productions = new List<Production>();
        }

        public Building(int buildingTypeId, int kingdomId, int time)
        {
            BuildingTypeId = buildingTypeId;
            BuildStartedAt= DateTime.Now;
            BuildCompletedAt = BuildStartedAt.AddSeconds(time);
            Productions = new List<Production>();
            KingdomId = kingdomId;
        }

        // Only to be used when creating a kingdom, so that the farm can be used immediately.
        public Building(int buildingTypeId, bool readymade)
        {
            BuildingTypeId = buildingTypeId;
            BuildStartedAt = DateTime.Now;
            BuildCompletedAt = DateTime.Now;
            Productions = new List<Production>();
        }
    }
}