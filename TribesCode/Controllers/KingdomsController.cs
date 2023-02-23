using dusicyon_midnight_tribes_backend.Models.APIRequests.Kingdoms;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers;

[Route("api/kingdoms")]
[ApiController]
[Authorize]

public class KingdomsController : ControllerBase
{
    private readonly IKingdomService _kingdomService;
    private readonly ITokenService _tokenService;
    
    public KingdomsController(IKingdomService kingdomService,ITokenService tokenService)
    {
        _kingdomService = kingdomService;
        _tokenService = tokenService;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var response = _kingdomService.GetAllKingdoms();
        if (response is not ErrorResponse error) return Ok(response);
        return StatusCode(error.StatusCode, error);
    }

    [HttpGet("get-all-my-kingdoms")]
    public ActionResult GetAllMyKingdoms ([FromHeader]string authorization)
    {
        int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

        var response = _kingdomService.GetAllMyKingdoms(playerId);

        if (response is ErrorResponse)
        {
            var error = response as ErrorResponse;

            return StatusCode(error.StatusCode, error);
        }

        return Ok(response);
    }


    [HttpGet("{id}")]
    public ActionResult Show([FromRoute] int id)
    {
        var response = _kingdomService.GetKingdomByID(id);
        if (response is not ErrorResponse error) return Ok(response);
        return StatusCode(error.StatusCode, error);
    }

     [HttpPost("create")]
     public ActionResult Create([FromHeader] string authorization, CreateKingdomRequest request)
     {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

        var response = _kingdomService.Create(request,playerId);
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
