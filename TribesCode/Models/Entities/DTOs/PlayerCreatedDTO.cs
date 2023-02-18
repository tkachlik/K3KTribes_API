namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class PlayerCreatedDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as PlayerCreatedDTO;

            if (other == null) return false;

            if (UserName != other.UserName) return false;
            
            return true;
        }
    }
}