using dusicyon_midnight_tribes_backend.Models.APIRequests.Buildings;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]

public class BuildingsController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IBuildingService _buildingService;

    public BuildingsController(ITokenService tokenService, IBuildingService building)
    {
        _tokenService = tokenService;
        _buildingService = building;
    }

    [HttpPost("create")]
    public ActionResult Create([FromHeader] string authorization, CreateBuildingRequest request)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

        var response = _buildingService.Create(request, playerId);
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

    [HttpPut("upgrade/{buildingId}")]
    public ActionResult Upgrade([FromHeader] string authorization, [FromRoute] int buildingId)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

        var response = _buildingService.Upgrade(buildingId, playerId);
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

    [HttpGet("show-construction-options/{kingdomId}")]
    public ActionResult ShowConstructionOptions([FromHeader] string authorization,[FromRoute] int kingdomId)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

        var response = _buildingService.ShowConstructionOptions(kingdomId, playerId);
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

    [HttpGet("show-available-upgrades/{kingdomId}")]
    public ActionResult ShowAvailableUpgrades([FromHeader] string authorization, [FromRoute] int kingdomId)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);
        
        var response = _buildingService.ShowAvailableUpgrades(kingdomId, playerId);

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

    [HttpGet("show-buildings-under-construction/{kingdomId}")]
    public ActionResult ShowBuildingsUnderConstruction([FromHeader] string authorization,
        [FromRoute] int kingdomId)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);
        
        var response = _buildingService.ShowBuildingsUnderConstruction(kingdomId, playerId);
        
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