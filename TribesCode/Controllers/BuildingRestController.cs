using dusicyon_midnight_tribes_backend.Models.APIRequests.BuildingRest;
using dusicyon_midnight_tribes_backend.Models.APIRequests.KingdomRest;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using dusicyon_midnight_tribes_backend.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers;
[Route("api/buildings")]
[ApiController]
[Authorize]

public class BuildingRestController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IBuildingService _building;

    public BuildingRestController(ITokenService tokenService, IBuildingService building)
    {
        _tokenService = tokenService;
        _building = building;
    }

    [HttpPost("create")]
    public ActionResult Create([FromHeader] string authorization, CreateBuildingRequest request)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

        var response = _building.Create(request, playerId);
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

    [HttpPost("upgrade")]
    public ActionResult Upgrade([FromHeader] string authorization, UpgradeBuildingRequest request)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

        var response = _building.Upgrade(request, playerId);
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

    [HttpPost("constructionOptions")]
    public ActionResult ConstructionOptions([FromHeader] string authorization,ConstructionOptionsRequest request)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

        var response = _building.ConstructionOptions(request, playerId);
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

    [HttpPost("showAvailableUpgrades")]
    public ActionResult ShowAvailableUpgrades([FromHeader] string authorization, ShowAvailableUpgradesRequest request)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);
        
        var response = _building.ShowAvailableUpgrades(request, playerId);

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

    [HttpPost("showBuildingsUnderConstruction")]
    public ActionResult ShowBuildingsUnderConstruction([FromHeader] string authorization,
        ShowBuildingsUnderConstructionRequest request)
    {
        var playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);
        
        var response = _building.ShowBuildingsUnderConstruction(request, playerId);
        
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