namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class ForgottenPassword
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }


        public ForgottenPassword() { }
        public ForgottenPassword(int playerID, string token, DateTime expiresAt)
        {
            PlayerId = playerID;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.Now;
        }
    }
}