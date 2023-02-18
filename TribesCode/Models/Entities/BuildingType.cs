using System.ComponentModel.DataAnnotations.Schema;

namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class BuildingType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }

        // This is the price of the building.
        [ForeignKey("BuildingCost")]
        public int BuildingCostId { get; set; }
        public BuildingCost BuildingCost { get; set; }

        // This is a list of actual buildings already built that are of this type.
        public List<Building> Buildings { get; set; }

        // This is a list of things this building type can produce.
        public List<ProductionOption>? ProductionOptions { get; set; }

        //public int ResourceTypeId { get; set; }
        //public ResourceType ResourceType {get;set;}

        public BuildingType () { }
        public BuildingType (int id, string name, int level, int buildingCostId)
        {
            Id = id;
            Name = name;
            Level = level;
            BuildingCostId = buildingCostId;
            Buildings = new List<Building> ();
        }

        //// This is for TOWNHALL ONLY because it has no production options.
        //public BuildingType(int id, string name, int level, int buildingCostId)
        //{
        //    Id = id;
        //    Name = name;
        //    Level = level;
        //    BuildingCostId = buildingCostId;
        //    Buildings = new List<Building>();
        //    ProductionOptions = null;
        //}
    }
}