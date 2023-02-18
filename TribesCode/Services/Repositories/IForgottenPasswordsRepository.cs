using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public interface IForgottenPasswordsRepository
    {
        void AddForgottenPassword(ForgottenPassword forgottenPassword);
        ForgottenPassword GetFPByTokenFromEmail(string tokenFromEmail);
        ForgottenPassword GetForgottenPasswordByPlayerObject(Player player);
        void UpdateForgottenPassword(ForgottenPassword forgottenPassword);
    }
}