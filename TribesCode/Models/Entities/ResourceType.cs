namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class ResourceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Resource> Resources { get; set; }
        public List<ProductionOption> ProductionOptions { get; set; }
        public List<UnitCost> UnitCosts { get; set; }

        public ResourceType() { }

        public ResourceType(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}