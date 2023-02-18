using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public interface IEmailVerificationsRepository
    {
        void AddEmailVerification(EmailVerification emailVerification);
        void UpdateEV(EmailVerification emailVerification);
        EmailVerification GetEmailVerificationByPlayerId(int playerId);
        EmailVerification GetEv_IncludingPlayer_ByTokenFromEmail(string tokenFromEmail);
        bool CheckEmailVerificationExistsByPlayerId(int id);
    }
}