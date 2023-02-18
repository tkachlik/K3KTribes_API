using dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services
{
    public interface IPlayerManagementService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHashed);
        IResponse Register(PlayerRegistrationRequest request);
        IResponse Login(PlayerLoginRequest request);
        IResponse VerifyPlayerEmail(int playerId);
        IResponse ResendEmailVerificationToken(int playerId);
        IResponse SendPasswordResetToken(SendPasswordResetTokenRequest request);
        IResponse ResetPassword(ResetPasswordRequest request);
    }
}