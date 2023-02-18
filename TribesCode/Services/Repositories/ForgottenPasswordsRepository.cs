using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public class ForgottenPasswordsRepository : IForgottenPasswordsRepository
    {
        private readonly IContext _context;

        public ForgottenPasswordsRepository (IContext context)
        {
            _context = context;
        }

        public void AddForgottenPassword(ForgottenPassword forgottenPassword)
        {
            _context.ForgottenPasswords
                .Add(forgottenPassword);
        }

        public ForgottenPassword GetForgottenPasswordByPlayerObject(Player player)
        {
            return _context.ForgottenPasswords
                .FirstOrDefault(fp => fp.Player == player);
        }

        public ForgottenPassword GetFPByTokenFromEmail(string tokenFromEmail)
        {
            return _context.ForgottenPasswords
                .FirstOrDefault(fp => fp.Token == tokenFromEmail);
        }

        public void UpdateForgottenPassword(ForgottenPassword forgottenPassword)
        {
            _context.ForgottenPasswords
                .Update(forgottenPassword);
        }
    }
}