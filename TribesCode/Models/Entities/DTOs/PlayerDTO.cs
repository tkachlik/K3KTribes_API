namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string? VerifiedAt { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as PlayerDTO;

            if (other == null) return false;

            if (Id != other.Id || UserName != other.UserName || VerifiedAt != other.VerifiedAt) return false;

            else return true;
        }
    }
}