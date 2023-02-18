namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class World
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public List<Kingdom>? Kingdoms { get; set; }

        public List<PlayerWorld>? PlayerWorlds { get; set; }

        public World() { }

        public World(string name)
        {
            Name = name;
            Kingdoms = new List<Kingdom>();
            PlayerWorlds = new List<PlayerWorld>();
        }
    }
}