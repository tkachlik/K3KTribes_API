using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IPlayerManagementService _playerManagementService;

        public EmailController(ITokenService tokenService, IPlayerManagementService playerManagementService)
        {
            _tokenService = tokenService;
            _playerManagementService = playerManagementService;
        }

        [HttpGet("verify/{token}")]
        public ActionResult VerifyPlayerEmailStep2([FromRoute]string token)
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

        [HttpPost("verify/resend"), Authorize]
        public ActionResult ResendEmailVerificationToken([FromHeader]string authorization)
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
    }
}