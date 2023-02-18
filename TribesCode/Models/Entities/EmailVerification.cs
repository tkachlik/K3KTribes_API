namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class EmailVerification
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public EmailVerification() { }

        public EmailVerification(int playerId, string token, DateTime expiresAt)
        {
            PlayerId = playerId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.Now;
        }
    }
}