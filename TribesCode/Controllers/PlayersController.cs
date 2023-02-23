using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ITokenService _tokenService;

        public PlayersController(IPlayerService playerService, ITokenService tokenService)
        {
            _playerService = playerService;
            _tokenService = tokenService;
        }

        [HttpGet("")]
        public ActionResult GetAllPlayers()
        {
            var response = _playerService.GetAllPlayers();

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult GetPlayerById([FromRoute]int id)
        {
            var response = _playerService.GetPlayerByID(id);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }

        [HttpGet("get-my-info"), Authorize]
        public ActionResult GetLoggedInPlayer([FromHeader] string authorization)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _playerService.GetPlayerByID(playerId);

            if (response is Error) 
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }
    }
}