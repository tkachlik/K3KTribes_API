using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public class EmailVerificationsRepository : IEmailVerificationsRepository
    {
        private readonly IContext _context;

        public EmailVerificationsRepository(IContext context)
        {
            _context = context;
        }

        public EmailVerification GetEmailVerificationByPlayerId(int playerId)
        {
            return _context.EmailVerifications
                .FirstOrDefault(ev => ev.PlayerId == playerId);
        }

        public EmailVerification GetEv_IncludingPlayer_ByTokenFromEmail(string tokenFromEmail)
        {
            return _context.EmailVerifications
                .Include(ev => ev.Player)
                .FirstOrDefault(ev => ev.Token == tokenFromEmail);
        }

        public bool CheckEmailVerificationExistsByPlayerId(int playerId)
        {
            return _context.EmailVerifications
                .Any(ev => ev.PlayerId == playerId);
        }

        public void AddEmailVerification(EmailVerification emailVerification)
        {
            _context.EmailVerifications
                .Add(emailVerification);
        }

        public void UpdateEV(EmailVerification emailVerification)
        {
            _context.EmailVerifications
                .Update(emailVerification);
        }
    }
}