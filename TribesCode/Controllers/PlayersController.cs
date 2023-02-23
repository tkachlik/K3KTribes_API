using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers
{
    [Route("api/players")]
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
        public ActionResult Index()
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
        public ActionResult Show([FromRoute]int id)
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
        public ActionResult GetPlayer([FromHeader] string authorization)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            return Ok(_playerService.GetPlayerByID(playerId));
        }
    }
}