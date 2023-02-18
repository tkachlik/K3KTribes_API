namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class MyKingdomDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string World { get; set; }
        public int WorldId { get; set; }
        
        public override bool Equals(object? obj)
        {
            var other = obj as MyKingdomDTO;

            if (other == null) return false;

            if (Id != other.Id || Name != other.Name || World != other.World || WorldId != other.WorldId) return false;

            else return true;
        }
    }
}