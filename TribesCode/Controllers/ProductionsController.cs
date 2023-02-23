using dusicyon_midnight_tribes_backend.Models.APIRequests.Productions;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ProductionsController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IProductionOptionsService _poService;
        private readonly IProductionService _prodService;

        public ProductionsController(ITokenService tokenService, IProductionOptionsService poService, IProductionService prodService)
        {
            _tokenService = tokenService;
            _poService = poService;
            _prodService = prodService;
        }

        [HttpGet("show-available-production-options/{kingdomId}")]
        public ActionResult ShowAvailableProductionOptions
            ([FromHeader]string authorization, [FromRoute]int kingdomId)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _poService.ShowAllAvailableProductionOptions(playerId, kingdomId);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }

        [HttpPost("produce-resource")]
        public ActionResult ProduceResource
            ([FromHeader] string authorization, [FromBody] ProduceResourceRequest request)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _prodService.ProduceResource(playerId, request);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }

        [HttpGet("show-all-uncollected-productions/{kingdomId}")]
        public ActionResult ShowAllUncollectedProductions
            ([FromHeader] string authorization, [FromRoute]int kingdomId)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _prodService.ShowAllUncollectedProductions(playerId, kingdomId);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }

        [HttpPatch("collect-production/{productionId}")]
        public ActionResult CollectProduction
            ([FromHeader] string authorization, [FromRoute] int productionId)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _prodService.CollectProduction(playerId, productionId);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }

        [HttpDelete("delete-production/{productionId}")]
        public ActionResult DeleteProduction
            ([FromHeader]string authorization, [FromRoute] int productionId)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _prodService.DeleteProduction(playerId, productionId);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }
    }
}