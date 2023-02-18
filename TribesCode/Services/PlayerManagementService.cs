namespace dusicyon_midnight_tribes_backend.Services;

using BCrypt.Net;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement;
using dusicyon_midnight_tribes_backend.Services.Repositories;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using AutoMapper;

public class PlayerManagementService : IPlayerManagementService
{
    private readonly IGenericRepository _repo;
    private readonly IPlayerRepository _playerRepo;
    private readonly IEmailVerificationsRepository _emailVerificationRepo;
    private readonly IForgottenPasswordsRepository _forgottenPasswordRepo;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public PlayerManagementService(IGenericRepository repo,
        IPlayerRepository playerRepo,
        IEmailVerificationsRepository emailVerificationRepo,
        IForgottenPasswordsRepository forgottenPasswordRepo,
        ITokenService tokenService,
        IEmailService emailService,
        IMapper mapper)
    {
        _repo = repo;
        _playerRepo = playerRepo;
        _emailVerificationRepo= emailVerificationRepo;
        _forgottenPasswordRepo= forgottenPasswordRepo;
        _tokenService = tokenService;
        _emailService = emailService;
        _mapper = mapper;
    }

    // PUBLIC INTERFACE METHODS
    public string HashPassword(string password)
    {
        return BCrypt.EnhancedHashPassword(password, hashType: HashType.SHA384);
    }

    public bool VerifyPassword(string password, string passwordHashed)
    {
        return BCrypt.EnhancedVerify(password, passwordHashed, hashType: HashType.SHA384);
    }

    public IResponse Register(PlayerRegistrationRequest request)
    {
        var passwordHashed = HashPassword(request.Password);
        string name = request.UserName;
        string email = request.Email;

        if (_playerRepo.CheckPlayerExistsByName(name) && _playerRepo.CheckEmailExists(email))
        {
            var response = new ErrorResponse(409, "UserName", "Exists already.", "Email", "Exists already.");

            return response;
        }

        else if (_playerRepo.CheckPlayerExistsByName(name))
        {
            var response = new ErrorResponse(409, "UserName", "Exists already.");

            return response;
        }

        else if (_playerRepo.CheckEmailExists(email))
        {
            var response = new ErrorResponse(409, "Email", "Exists already.");

            return response;
        }

        else
        {
            var newPlayer = new Player()
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHashed = passwordHashed
            };

            _playerRepo.AddPlayer(newPlayer);

            if (!_repo.Save())
            {
                return new SaveChangesErrorResponse();
            }

            var response = new PlayerRegistrationResponse(_mapper.Map<PlayerCreatedDTO>(newPlayer));
            return response;
        }
    }

    public IResponse Login(PlayerLoginRequest request)
    {
        var player = _playerRepo.GetPlayerByName(request.UserName);

        if (player != null)
        {
            if (VerifyPassword(request.Password, player.PasswordHashed))
            {
                var response = new PlayerLoginResponse()
                {
                    AuthenticationToken = _tokenService.GenerateLoginToken(player)
                };

                return response;
            }
            else
            {
                var response = new ErrorResponse(401, "Password", "Doesn't match the username.");

                return response;
            }
        }
        else
        {
            var response = new ErrorResponse(404, "UserName", "Doesn't exist.");

            return response;
        }
    }

    public IResponse VerifyPlayerEmail(int playerId)
    {
        if (!_emailVerificationRepo.CheckEmailVerificationExistsByPlayerId(playerId))
        {
            string token;
            DateTime expiration;

            _tokenService.GenerateOneHourToken(playerId, out token, out expiration);

            var emailVerification = new EmailVerification(playerId, token, expiration);

            _emailVerificationRepo.AddEmailVerification(emailVerification);

            if (!_repo.Save())
            {
                return new SaveChangesErrorResponse();
            }

            var player = _playerRepo.GetPlayerById(playerId);

            _emailService.SendEmailWithEmailVerificationToken(token, player.Email, player.UserName);

            var response = new EmailVerificationResponse();

            return response;
        }
        else
        {
            var response = new ErrorResponse(400, "VerificationToken", "Already sent. --- If you need the token resend, please use the 'resend verification token' endpoint.");

            return response;
        }
    }

    public IResponse ResendEmailVerificationToken(int playerId)
    {
        var currentEV = _emailVerificationRepo.GetEmailVerificationByPlayerId(playerId);

        if (currentEV == null)
        {
            var response404 = new ErrorResponse(404, "VerificationToken", "Not send yet. --- Please use the 'verify-your-email' endpoint.");

            return response404;
        }

        var player = _playerRepo.GetPlayerById(playerId);

        if (player.VerifiedAt != null)
        {
            var response400 = new ErrorResponse(400, "Email", "Verified already.");

            return response400;
        }

        string token;
        DateTime expiration;

        _tokenService.GenerateOneHourToken(playerId, out token, out expiration);

        currentEV.Token = token;
        currentEV.ExpiresAt = expiration;

        _emailVerificationRepo.UpdateEV(currentEV);

        if(!_repo.Save())
        {
            return new SaveChangesErrorResponse();
        };

        _emailService.SendEmailWithEmailVerificationToken(token, player.Email, player.UserName);

        var responseOk = new ResendEmailVerificationTokenResponse();

        return responseOk;
    }

    public IResponse SendPasswordResetToken(SendPasswordResetTokenRequest request)
    {
        string email = request.Email;

        if (!_playerRepo.CheckEmailExists(email))
        {
            return new ErrorResponse(404, "Email", "No such email found. --- Only registered and verified emails can be used for password reset");
        }
        else if (!_playerRepo.CheckPlayerIsVerified(email))
        {
            return new ErrorResponse(400, "Email", "The entered email address has not been verified yet! --- Only verified emails can be used for password reset.");
        }
        else
        {
            var player = _playerRepo.GetOnlyVerifiedPlayerByEmail(email);

            if (player == null)
            {
                return new ErrorResponse(500, "", "Internal server error.");
            }

            string token;
            DateTime expiration;

            _tokenService.GenerateOneHourToken(player.Id, out token, out expiration);

            string overwriteStatus = OverwriteForgottenPasswordIfExistsAndSendToken(player, token, expiration);

            switch (overwriteStatus)
            {
                case "true": return new PasswordResetTokenResponse();

                case "dbFail": return new SaveChangesErrorResponse();

                case "false": return AddNewForgottenPasswordToDb(player, token, expiration);

                default: return new ErrorResponse(400, "", "Unknown error.");
            }
        }
    }

    public IResponse ResetPassword(ResetPasswordRequest request)
    {
        int playerIdFromToken;
        ForgottenPassword forgottenPassword;
        var tokenCheckResult = _tokenService.CheckPasswordResetTokenValidity(request.Token, out playerIdFromToken, out forgottenPassword);

        switch (tokenCheckResult)
        {
            case "invalid": return new ErrorResponse(400, "Token", "Invalid.");

            case "expired": return new ErrorResponse(400, "Token", "Expired.");

            case "Ok":
                var pswHashed = HashPassword(request.NewPassword);
                var player = _playerRepo.GetPlayerById(playerIdFromToken);
                player.PasswordHashed = pswHashed;

                forgottenPassword.ExpiresAt = null; // nullify to make clear that psw is reset

                _forgottenPasswordRepo.UpdateForgottenPassword(forgottenPassword);

                _playerRepo.UpdatePlayer(player);

                if(!_repo.Save())
                {
                    return new SaveChangesErrorResponse();
                }

                return new OkResponse();

            default: return new ErrorResponse(400, "", "Unknown error has occurred.");
        }
    }

    // PRIVATE HELPER METHODS

    private string OverwriteForgottenPasswordIfExistsAndSendToken(Player player, string token, DateTime expiration)
    {
        var forgottenPassword = _forgottenPasswordRepo.GetForgottenPasswordByPlayerObject(player);

        if (forgottenPassword == null)
        {
            return "false";
        }

        forgottenPassword.Token = token;
        forgottenPassword.ExpiresAt = expiration;

        _forgottenPasswordRepo.UpdateForgottenPassword(forgottenPassword);

        if (!_repo.Save())
        {
            return "dbFail";
        }

        _emailService.SendEmailWithPasswordResetToken(token, player.Email, player.UserName);

        return "true";
    }

    private IResponse AddNewForgottenPasswordToDb(Player player, string token, DateTime expiration)
    {
        var forgottenPassword = new ForgottenPassword(player.Id, token, expiration);

        _forgottenPasswordRepo.AddForgottenPassword(forgottenPassword);

        if (!_repo.Save())
        {
            return new SaveChangesErrorResponse();
        }

        _emailService.SendEmailWithPasswordResetToken(token, player.Email, player.UserName);

        return new PasswordResetTokenResponse();
    }
}