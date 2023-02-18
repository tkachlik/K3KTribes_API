namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class WorldDTO 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int KingdomCount { get; set; }
        public List<string> PlayerNames { get; set; }

        public WorldDTO() { }

        public WorldDTO(int id, string name, int kingdomCount)
        {
            Id = id;
            Name = name;
            KingdomCount = kingdomCount;
            PlayerNames = new List<string>();
        }

        public override bool Equals(object? obj)
        {
            var other = obj as WorldDTO;

            if (other == null) return false;

            if (Id != other.Id || Name != other.Name || KingdomCount != other.KingdomCount || PlayerNames.Count != other.PlayerNames.Count) return false;

            else return true;
        }
    }
}
