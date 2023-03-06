using dusicyon_midnight_tribes_backend.Models.APIRequests.WorldRest;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Worlds;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorldsController : ControllerBase
    {
        private readonly IWorldService _worldService;
        private readonly ITokenService _tokenService;
       
        public WorldsController(IWorldService worldService, ITokenService tokenService)
        {
            _worldService = worldService;
            _tokenService = tokenService;
        }

        [HttpGet("")]
        public ActionResult GetAllWorlds()
        {
            var response = _worldService.GetAllWorlds();

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;
                return StatusCode(error.StatusCode, error);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult GetWorldById (int id)
        {
            var response = _worldService.GetWorldById(id);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;
                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);

        }

        [HttpPost("create")]
        public ActionResult Create([FromHeader] string authorization,[FromBody] CreateWorldRequest createWorldRequest)
        {
            var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);
            var response = _worldService.CreateWorld(createWorldRequest, playerId);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;
                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
            
        }
    }
}
