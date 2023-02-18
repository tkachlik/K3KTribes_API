using dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Services.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace dusicyon_midnight_tribes_backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IGenericRepository _repo;
        private readonly IEmailVerificationsRepository _evRepo;
        private readonly IForgottenPasswordsRepository _fpRepo;

        public TokenService(IConfiguration config,
            IGenericRepository repo,
            IEmailVerificationsRepository evRepo,
            IForgottenPasswordsRepository fpRepo)
        {
            _config = config;
            _repo = repo;
            _evRepo = evRepo;
            _fpRepo = fpRepo;
        }

        public int ReadPlayerIdFromTokenInHeader(string authorization)
        {
            string jwt = authorization.Substring(7);
            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            int playerId = Int32.Parse(token.Claims.FirstOrDefault(c => c.Type == "nameid").Value);

            return playerId;
        }

        public string GenerateLoginToken(Player player)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.NameId, player.Id.ToString())
            };

            var expiration = DateTime.Now.AddDays(3);

            var jwt = new JwtSecurityToken(issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: signature);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public void GenerateOneHourToken(int playerId, out string token, out DateTime expiration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.NameId, playerId.ToString())
            };

            expiration = DateTime.Now.AddMinutes(60);

            var jwt = new JwtSecurityToken(issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: signature);

            token = new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public IResponse CheckEmailVerificationTokenValidity(string tokenFromEmail)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenFromEmail);
            var playerIdFromToken = Int32.Parse(token.Claims.FirstOrDefault(c => c.Type == "nameid").Value);

            var emailVerification = _evRepo.GetEv_IncludingPlayer_ByTokenFromEmail(tokenFromEmail);

            if (emailVerification == null || emailVerification.PlayerId != playerIdFromToken)
            {
                return new ErrorResponse(400, "VerificationToken", "Invalid token.");
            }
            else if (emailVerification.Player.VerifiedAt != null)
            {
                return new ErrorResponse(400, "Email", "Verified already.");
            }
            else if (emailVerification.ExpiresAt <= DateTime.Now)
            {
                return new ErrorResponse(400, "Email", "Verified already.");
            }
            else
            {
                emailVerification.ExpiresAt = null;
                emailVerification.Player.VerifiedAt = DateTime.Now;

                _evRepo.UpdateEV(emailVerification);

                if (!_repo.Save())
                {
                    return new SaveChangesErrorResponse();
                }

                return new OkResponse();
            }

        }

        public string CheckPasswordResetTokenValidity(string tokenFromEmail, out int playerIdFromToken, out ForgottenPassword forgottenPassword)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenFromEmail);
            playerIdFromToken = Int32.Parse(token.Claims.FirstOrDefault(c => c.Type == "nameid").Value);

            forgottenPassword = _fpRepo.GetFPByTokenFromEmail(tokenFromEmail);

            if (forgottenPassword == null || forgottenPassword.PlayerId != playerIdFromToken)
            {
                return "invalid";
            }
            else if (forgottenPassword.ExpiresAt == null || forgottenPassword.ExpiresAt < DateTime.Now)
            {
                return "expired";
            }
            else
            {
                return "Ok";
            }
        }
    }
}
