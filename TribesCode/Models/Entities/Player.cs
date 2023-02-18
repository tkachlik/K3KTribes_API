namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public List<EmailVerification>? EmailVerifications { get; set; }

        public List<ForgottenPassword>? ForgottenPasswords { get; set; }

        public List<PlayerWorld>? PlayerWorlds { get; set; }  

        public List<Kingdom>? Kingdoms { get; set; }


        public Player() { }

        public Player(int id, string userName, string email, string passwordHashed)
        {
            Id = id;
            UserName = userName;
            Email = email;
            PasswordHashed = passwordHashed;
            
            EmailVerifications = new List<EmailVerification>();
            ForgottenPasswords = new List<ForgottenPassword>();
            PlayerWorlds = new List<PlayerWorld>();
            Kingdoms = new List<Kingdom>();
        }
    }


}