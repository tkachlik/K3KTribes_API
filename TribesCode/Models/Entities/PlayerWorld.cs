namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class PlayerWorld
    {
        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public int WorldId { get; set; } 
        public World? World { get; set; }
    }
}