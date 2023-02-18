using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services
{
    public interface ITokenService
    {
        int ReadPlayerIdFromTokenInHeader(string authorization);
        string GenerateLoginToken(Player player);
        void GenerateOneHourToken(int playerId, out string token, out DateTime expiration);
        IResponse CheckEmailVerificationTokenValidity(string tokenFromEmail);
        string CheckPasswordResetTokenValidity(string tokenFromEmail, out int playerIdFromToken, out ForgottenPassword forgottenPassword);
    }
}