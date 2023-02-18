using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers
{
    [Route("api/players")]
    [ApiController]
    [Authorize]
    public class PlayerRestController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerRestController(IPlayerService playerService)
        {
            _playerService = playerService;
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
    }
}