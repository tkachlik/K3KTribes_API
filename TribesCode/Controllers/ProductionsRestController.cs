using dusicyon_midnight_tribes_backend.Models.APIRequests.ProductionsRest;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dusicyon_midnight_tribes_backend.Controllers
{
    [Route("api/productions")]
    [ApiController, Authorize]
    public class ProductionsRestController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IProductionOptionsService _poService;
        private readonly IProductionService _prodService;

        public ProductionsRestController(ITokenService tokenService, IProductionOptionsService poService, IProductionService prodService)
        {
            _tokenService = tokenService;
            _poService = poService;
            _prodService = prodService;
        }

        [HttpPost("show-available-production-options-of-kingdom")]
        public ActionResult ShowAvailableProductionOptions
            ([FromHeader]string authorization, [FromBody]ShowAvailableProductionOptionsRequest request)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _poService.ShowAllAvailableProductionOptions(playerId, request.KingdomId);

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

        [HttpPost("show-all-uncollected-productions")]
        public ActionResult ShowAllUncollectedProductions
            ([FromHeader] string authorization, [FromBody] ShowAllUncollectedProductionsRequest request)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _prodService.ShowAllUncollectedProductions(playerId, request);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }

        [HttpPost("collect-production")]
        public ActionResult CollectProduction
            ([FromHeader] string authorization, [FromBody] CollectProductionRequest request)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _prodService.CollectProduction(playerId, request);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }

        [HttpPost("delete-production")]
        public ActionResult DeleteProduction
            ([FromHeader]string authorization, [FromBody] DeleteProductionRequest request)
        {
            int playerId = _tokenService.ReadPlayerIdFromTokenInHeader(authorization);

            var response = _prodService.DeleteProduction(playerId, request);

            if (response is ErrorResponse)
            {
                var error = response as ErrorResponse;

                return StatusCode(error.StatusCode, error);
            }

            return Ok(response);
        }
    }
}