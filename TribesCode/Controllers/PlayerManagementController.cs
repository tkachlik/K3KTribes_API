using dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerManagementController : ControllerBase
    {
        private readonly IPlayerManagementService _playerManagementService;
        private readonly ITokenService _tokenService;

        public PlayerManagementController(IPlayerManagementService playerManagementService, ITokenService tokenService)
        {
            _playerManagementService = playerManagementService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public ActionResult RegisterNewPlayer(PlayerRegistrationRequest request)
        {
            var response = _playerManagementService.Register(request);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;
                return StatusCode(error.StatusCode, error);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost("login")]
        public ActionResult LoginPlayer(PlayerLoginRequest request)
        {
            var response = _playerManagementService.Login(request);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("verify-your-email"), Authorize]
        public ActionResult VerifyPlayerEmailStep1([FromHeader] string authorization)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);
            var response = _playerManagementService.VerifyPlayerEmail(playerId);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("verify-your-email/{token}")]
        public ActionResult VerifyPlayerEmailStep2([FromRoute] string token)
        {
            var response = _tokenService.CheckEmailVerificationTokenValidity(token);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("resend-email-verification-token"), Authorize]
        public ActionResult ResendEmailVerificationToken([FromHeader] string authorization)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _playerManagementService.ResendEmailVerificationToken(playerId);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost("reset-password-request")]
        public ActionResult SendPasswordResetToken(SendPasswordResetTokenRequest request)
        {
            var result = _playerManagementService.SendPasswordResetToken(request);

            if (result is ErrorResponse)
            {
                var error = result as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("reset-password/{token}")]
        public ActionResult PasswordResetTokenDisplay([FromRoute] string token)
        {
            var result = new PasswordResetTokenDisplayResponse(token);

            return Ok(result);
        }

        [HttpPost("reset-password")]
        public ActionResult ResetPassword(ResetPasswordRequest request)
        {
            var result = _playerManagementService.ResetPassword(request);

            if (result is ErrorResponse)
            {
                var error = result as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}