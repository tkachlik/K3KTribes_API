using AutoMapper;
using dusicyon_midnight_tribes_backend.Models.APIRequests.Productions;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Productions;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using dusicyon_midnight_tribes_backend.Services.Repositories;

namespace dusicyon_midnight_tribes_backend.Services
{
    public class ProductionService : IProductionService
    {
        private readonly IGenericRepository _repo;
        private readonly IKingdomRepository _kingdomRepo;
        private readonly IProductionRepository _productionRepo;
        private readonly IProductionOptionRepository _productionOptionRepo;
        private readonly IBuildingRepository _buildingRepo;
        private readonly IResourceRepository _resourceRepo;
        private readonly IMapper _mapper;

        public ProductionService(IGenericRepository repo,
            IKingdomRepository kingdomRepo,
            IProductionRepository prodRepo,
            IProductionOptionRepository poRepo,
            IBuildingRepository buildingRepository,
            IResourceRepository resourceRepository,
            IMapper mapper)
        {
            _repo = repo;
            _kingdomRepo = kingdomRepo;
            _productionRepo = prodRepo;
            _productionOptionRepo = poRepo;
            _buildingRepo = buildingRepository;
            _resourceRepo = resourceRepository;
            _mapper = mapper;
        }


        // PUBLIC INTERFACE METHODS

        public IResponse ProduceResource(int playerId, ProduceResourceRequest request)
        {
            int buildingId = request.BuildingId;
            int productionOptionId = request.ProductionOptionId;
            var building = _buildingRepo.GetBuildingById(buildingId);
            var productionOption = _productionOptionRepo.GetProductionOptionById(productionOptionId);

            var error = PerformNullAndGameLogicChecks_ForProduceResource(playerId, building, productionOption);

            if (error != null)
            {
                return error;
            }

            int kingdomId = building.KingdomId;
            int prodTime = _productionOptionRepo.GetProductionTimeByProductionOptionId(productionOptionId);
            var production = new Production(kingdomId, buildingId, productionOptionId);
            production.CompletedAt = production.StartedAt.AddSeconds(prodTime);

            _productionRepo.AddProduction(production);

            var food = _resourceRepo.GetFoodByKingdomId(kingdomId);
            food.Amount -= building.BuildingType.Level;

            _resourceRepo.UpdateResource(food);

            if (!_repo.Save())
            {
                return new SaveChangesErrorResponse();
            }

            var prodDTO = _mapper.Map<ProductionCreatedDTO>(_productionRepo.GetProductionById(production.Id));

            return new ProduceResourceResponse(prodDTO);
        }

        public IResponse ShowUncollectedProductions(int playerId, int kingdomId)
        {
            if (!_kingdomRepo.CheckIfKingdomExistsById(kingdomId))
            {
                return new ErrorResponse(404, "KingdomId", "No such kingdom exists.");
            }

            if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, kingdomId))
            {
                return new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to collect its productions!");
            }

            var productions = _productionRepo.GetAllUncollectedProductions(kingdomId);

            var productionsDTOs = productions.Select(p => _mapper.Map<ProductionDTO>(p)).ToList();

            return new ShowUncollectedProductionsResponse(productionsDTOs);
        }

        public IResponse CollectProduction(int playerId, int productionId)
        {
            var production = _productionRepo.GetProductionById(productionId);

            if (production == null)
            {
                return new ErrorResponse(404, "ProductionId", "No such Production exists.");
            }

            int kingdomId = production.KingdomId;
            int resourceTypeId = (int)production.ProductionOption.ResourceTypeId;
            int resourceAmountProduced = production.ProductionOption.Amount;
            var resource = _resourceRepo.GetResourceByKingdomIdAndResourceTypeId(kingdomId, resourceTypeId);

            var error = PerformGameLogicChecks_ForCollectProduction(playerId, kingdomId, production, resourceAmountProduced);

            if (error != null)
            {
                return error;
            }

            resource.Amount += resourceAmountProduced;
            production.Collected = true;

            _resourceRepo.UpdateResource(resource);

            _productionRepo.UpdateProduction(production);

            if (!_repo.Save())
            {
                return new SaveChangesErrorResponse();
            }

            var productionDTO = _mapper.Map<ProductionCollectedOrDeletedDTO>(production);

            return new CollectProductionResponse(productionDTO);
        }

        public IResponse DeleteProduction(int playerId, int productionId)
        {
            var production = _productionRepo.GetProductionById(productionId);

            if (production == null)
            {
                return new ErrorResponse(404, "ProductionId", "No such production exists.");
            }

            var error = PerformGameLogicChecks_ForDeleteProduction(playerId, production);

            if (error != null)
            {
                return error;
            }

            int kingdomId = production.KingdomId;

            var food = _resourceRepo.GetFoodByKingdomId(kingdomId);
            int foodAmountToReturn;
            bool isEverythingReturned;

            CalculateFoodAmountToReturnAfterProductionCancellation
                (production, kingdomId, out foodAmountToReturn, out isEverythingReturned);
            
            food.Amount += foodAmountToReturn;

            _resourceRepo.UpdateResource(food);

            _productionRepo.DeleteProduction(production);

            if (!_repo.Save())
            {
                return new SaveChangesErrorResponse();
            }

            var productionDTO = _mapper.Map<ProductionCollectedOrDeletedDTO>(production);

            if (isEverythingReturned)
            {
                return new DeleteProductionResponse(productionDTO);
            }
            
            return new DeleteProductionResponse(foodAmountToReturn, productionDTO);
            
        }


        // PRIVATE HELPER METHODS

        private ErrorResponse PerformNullAndGameLogicChecks_ForProduceResource
            (int playerId, Building? building, ProductionOption? productionOption)
        {
            // NULL CHECKS
            if (building == null && productionOption == null)
            {
                return new ErrorResponse(404, "BuildingId", "No such building exists.",
                    "ProductionOptionId", "No such Production Option exists.");
            }

            if (building == null)
            {
                return new ErrorResponse(404, "BuildingId", "No such building exists.");
            }

            if (productionOption == null)
            {
                return new ErrorResponse(404, "ProductionOptionId", "No such Production Option exists.");
            }

            // GAME LOGIC CHECKS
            if (building.BuildingType.ProductionOptions == null ||
                !building.BuildingType.ProductionOptions.Contains(productionOption))
            {
                return new ErrorResponse(404, "ProductionOptionId", "This Building Type does not support this Production Option.");
            }

            int kingdomId = building.KingdomId;

            if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, kingdomId))
            {
                return new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to use its Buildings to produce resources!");
            }

            // Check if the Kingdom has enough food to cover the consumption cost (== building lvl).
            int foodAmount = _resourceRepo.GetFoodAmount(kingdomId);
            int consumption = building.BuildingType.Level;

            if (consumption > foodAmount)
            {
                return new ErrorResponse(404, "", $"There is not enough food in your kingdom to produce this resource. Each time you produce a resource, it will require an amount of food equal to the level of the Building that produces the resource.");
            }

            // Check if the Building is already producing the same resources (and the production has not yet been completed).
            bool isAlreadyProducing = building.Productions
                                              .Any(p => p.ProductionOptionID == productionOption.Id
                                                     && p.CompletedAt > DateTime.Now);

            if (isAlreadyProducing)
            {
                return new ErrorResponse(400, "", "This building is already producing this type of resource. You can only start new production when the one running is finished.");
            }

            return null;
        }

        private ErrorResponse PerformGameLogicChecks_ForCollectProduction
            (int playerId, int kingdomId, Production production, int resourceAmountProduced)
        {
            if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, kingdomId))
            {
                return new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to collect its Productions!");
            }

            if (production.Collected)
            {
                return new ErrorResponse(404, "ProductionId", "This Production's result has already been collected and added to your kingdom's resources. You cannot collect it again.");
            }

            if (production.CompletedAt > DateTime.Now) // Is the Production complete?
            {
                return new ErrorResponse(401, "ProductionId", "This Production is still in the making, so you cannot collect it.");
            }

            // Make sure collection won't be over max storage capacity.
            int resourceTypeId = (int)production.ProductionOption.ResourceTypeId;
            int maxStorage = _kingdomRepo.GetKingdomById(kingdomId).MaxStorage;
            int currentResourceLevel = _resourceRepo.GetResourceAmount(kingdomId, resourceTypeId);
            int storageAfterCollection = currentResourceLevel + resourceAmountProduced;

            if (storageAfterCollection > maxStorage)
            {
                return new ErrorResponse(401, "", "Your kingdom does not have enough free Storage to collect this.");
            }

            return null;
        }

        private ErrorResponse PerformGameLogicChecks_ForDeleteProduction
            (int playerId, Production production)
        {
            if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, production.KingdomId))
            {
                return new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to delete its Productions!");
            }

            if (production.CompletedAt < DateTime.Now) // Is the Production complete?
            {
                return new ErrorResponse(401, "ProductionId", "This Production has already been completed and is ready to be collected now. Once the Production process is finished, you cannot cancel it.");
            }

            return null;
        }

        private void CalculateFoodAmountToReturnAfterProductionCancellation(Production production, int kingdomId, out int foodAmountActuallyReturned, out bool isEverythingReturned)
        {
            int maxStorage = production.Kingdom.MaxStorage;
            int originalConsumptionCost = production.Building.BuildingType.Level;
            int currentFoodAmount = _resourceRepo.GetFoodAmount(kingdomId);
            int foodStorageAfterProdCancellation = currentFoodAmount + originalConsumptionCost;

            if (foodStorageAfterProdCancellation > maxStorage)
            {
                foodAmountActuallyReturned = originalConsumptionCost - (foodStorageAfterProdCancellation - maxStorage);
                isEverythingReturned = false;
            }
            else
            {
                foodAmountActuallyReturned = originalConsumptionCost;
                isEverythingReturned = true;
            }
        }
    }
}