using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Services.Repositories;

namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class Kingdom
    {
        public int Id { get; set; }
        public string Name { get; set; }        

        public int WorldId { get; set; }
        public World World { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int Coordinate_X { get; set; }        
        public int Coordinate_Y { get; set; }

        public List<Army> Armies { get; set; }
        
        public List<Resource> Resources { get; set; }
        public int MaxStorage { get; set; } // determines max amount of EACH resource

        public List<Building> Buildings { get; set; }

        public List<Production> Productions { get; set; }

        public int Loyalty { get; set; } = 100;
        public int Research { get; set; } = 1;
        
        // This will be calculated as all units' attack points together.
        public int Attack { get; set; }

        // This will be calculated as all units' defense points + buildings defense (i. e. Townhall and Wall, if exists) + defense bonus % (from Wall).
        public int Defense { get; set; }

        public Kingdom() { }

        public Kingdom(string name, int worldId, int playerId, int coordinate_X, int coordinate_Y )
        {
            Name = name;
            WorldId = worldId;
            PlayerId = playerId;
            Coordinate_X = coordinate_X;
            Coordinate_Y = coordinate_Y;
            Armies = new List<Army>();

            Resources = new List<Resource>()
            {
                new Resource (1000, 1), // Gold
                new Resource (1000, 2)  // Food
            };

            Buildings = new List<Building>();
            Productions = new List<Production>();
        }

        
    }
}