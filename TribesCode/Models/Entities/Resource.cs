namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class Resource
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        
        public int ResourceTypeId { get; set; }
        public ResourceType ResourceType { get; set; }

        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }

        public List<Production> Productions { get; set; }

        public Resource() { }

        public Resource(int amount, int resourceTypeId)
        {
            Amount = amount;
            ResourceTypeId = resourceTypeId;
            Productions = new List<Production>();
        }
    }
}