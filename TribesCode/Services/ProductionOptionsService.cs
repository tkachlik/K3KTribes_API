﻿using AutoMapper;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Productions;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using dusicyon_midnight_tribes_backend.Services.Repositories;

namespace dusicyon_midnight_tribes_backend.Services
{
    public class ProductionOptionService : IProductionOptionService
    {
        private readonly IProductionOptionRepository _productionOptionRepo;
        private readonly IKingdomRepository _kingdomRepo;
        private readonly IMapper _mapper;

        public ProductionOptionService(IProductionOptionRepository poRepo, IKingdomRepository kingdomRepo, IMapper mapper)
        {
            _productionOptionRepo = poRepo;
            _kingdomRepo = kingdomRepo;
            _mapper = mapper;
        }

        public IResponse ShowAvailableProductionOptions(int playerId, int kingdomId)
        {
            if(kingdomId < 1)
            {
                return new ValidationResultModel("KingdomId", "Must be a positive integer greater than 0.");
            }
            
            if(!_kingdomRepo.CheckIfKingdomExistsById(kingdomId))
            {
                return new ErrorResponse(404, "KingdomId", "No such kingdom found.");
            }
            
            if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, kingdomId))
            {
                return new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to view its Production Options!");
            }

            var productionOptions = _productionOptionRepo.GetAllAvailableProductionOptions(playerId, kingdomId);

            if (productionOptions == null)
            {
                return new ErrorResponse(500, "", "Internal server error.");
            }

            var result = productionOptions.Select(po => _mapper.Map<ProductionOptionsAvailableOnCompletedBuildingsDTO>(po))
                .ToList();

            return new ShowAvailableProductionOptionsResponse(result);
        }
    }
}