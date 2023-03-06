using AutoMapper;
using dusicyon_midnight_tribes_backend.Domain.GameConfig;
using dusicyon_midnight_tribes_backend.Models.APIRequests.Buildings;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Buildings;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using dusicyon_midnight_tribes_backend.Services.Repositories;

namespace dusicyon_midnight_tribes_backend.Services;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepo;
    private readonly IResourceRepository _resourceRepo;
    private readonly IGenericRepository _repo;
    private readonly IKingdomRepository _kingdomRepo;
    private readonly IGameConfig _gameConfig;
    private readonly IMapper _mapper;
    private readonly IBuildingTypeRepository _buildingTypeRepo;

    public BuildingService(IBuildingRepository building,IResourceRepository resource, IGenericRepository repo,IKingdomRepository kingdomRepo, IGameConfig gameConfig,IMapper mapper, IBuildingTypeRepository buildingTypeRepo)
    {
        _kingdomRepo = kingdomRepo;
        _buildingRepo = building;
        _resourceRepo = resource;
        _repo = repo;
        _gameConfig = gameConfig;
        _mapper = mapper;
        _buildingTypeRepo = buildingTypeRepo;
    }
    private bool CheckResources(int kingdomId,CreateBuildingRequest request) // checks if you have resources
    {                                                                        // if you do, then it will remove the funds required
        if (request.BuildingType.ToLower()  == _gameConfig.GetBuildingTypeName(0).ToLower())
        {
            if (_resourceRepo.GetGoldAmount(kingdomId) >
                _resourceRepo.CheckPrice(_buildingRepo.CheckTownHallLevel(kingdomId), request.BuildingType))
            {
                _resourceRepo.DecreaseGold(kingdomId,_resourceRepo.CheckPrice(_buildingRepo.CheckTownHallLevel(kingdomId), request.BuildingType),_buildingRepo.CheckTownHallLevel(kingdomId));
                return false;
            }
        }
        
        if (request.BuildingType.ToLower()  == _gameConfig.GetBuildingTypeName(1).ToLower())
        {
            if (_resourceRepo.GetGoldAmount(kingdomId) >
                _resourceRepo.CheckPrice(_buildingRepo.CheckTownHallLevel(kingdomId), request.BuildingType))
            {
                _resourceRepo.DecreaseGold(kingdomId,_resourceRepo.CheckPrice(_buildingRepo.CheckTownHallLevel(kingdomId), request.BuildingType),_buildingRepo.CheckTownHallLevel(kingdomId));
                return false;
            }

        }
        return true;
    }
    
    public bool IsTownhall(int buildingId)
    {
        var building = _buildingRepo.GetBuildingById(buildingId);
        return building.BuildingType.Name == _gameConfig.GetTownhallName();
    }
    
    public void UpgradeKingdomStorageCapacity(int kingdomId)
    {
        var kingdom = _kingdomRepo.GetKingdomById(kingdomId);
        kingdom.MaxStorage += _gameConfig.GetMaxStorageStep();
    }
    
    public IResponse Create(CreateBuildingRequest request, int playerId)
    {
        if (_buildingRepo.CheckTownHallLevel(request.KingdomId) == 0)
        {
            var response = new ErrorResponse(404, "KingdomId", "Does not exist");

            return response;
        }
        
        if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, request.KingdomId))
        {
            var response = new ErrorResponse(401, "Authorization", "This kingdom does not belong to you");

            return response;
        }

        if (CheckResources(request.KingdomId, request))
        {
            var response = new ErrorResponse(402, "Resources", "You dont have enough gold or food");

            return response;
        }

        if (_buildingRepo.CheckBuildingLimit(request.KingdomId, request.BuildingType))
        {
            var response = new ErrorResponse(413, "Buildings", "You cannot build over the building limit");

            return response;
        }
        
        else
        {
            var building = new Building(_buildingRepo.GetBuildingTypeId(request.BuildingType, request.KingdomId), request.KingdomId,_buildingRepo.GetBuildingTime(_buildingRepo.GetBuildingTypeId(request.BuildingType, request.KingdomId)));
            _buildingRepo.CreateBuilding(building);

            if (!_repo.Save())
            {
                var error = new SaveChangesErrorResponse();
            }

            var response = new CreateBuildingResponse(_mapper.Map<BuildingCreatedDTO>(building));
            return response;
        }
    }
    
    private bool CheckUpgradeFunds(int kingdomId, int buildingId)
    {
        var building = _buildingRepo.GetBuildingById(buildingId);
        if (_resourceRepo.GetGoldAmount(kingdomId) > building.BuildingType.BuildingCost.Amount) 
        {
            var townhall = _buildingRepo.CheckTownHallLevel(kingdomId);
            var value = building.BuildingType.BuildingCost.Amount;
            
            if(IsTownhall(buildingId))
            {
                _resourceRepo.DecreaseGold(kingdomId,value,townhall + 1);
                return false;
            }
            _resourceRepo.DecreaseGold(kingdomId,value,townhall);
            return false;
        }
        return true;
    }
    private bool BuildingTownhallComparison(Building building)
    {
        var townhallLevel = _buildingRepo.CheckTownHallLevel(building.KingdomId);

        if (building.BuildingType.Level >= townhallLevel)
        {
            return true;
        }
        return false;
    }



    public IResponse Upgrade(int buildingId, int playerId)
    {
        if (_buildingRepo.DoesBuildingExist(buildingId))
        {
            var response = new ErrorResponse(404, "building", "does not exist");

            return response;
        }

        if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, _buildingRepo.GetKingdomIdFromBuildingId(buildingId)))
        {
            var response = new ErrorResponse(401, "Authorization",
                "The building inside of this kingdom does not belong to you");

            return response;
        }

        if (_buildingRepo.CheckIfBuildingIsUnderConstruction(buildingId))
        {
            var response = new ErrorResponse(403, "Forbidden", "This building is still under construction");

            return response;
        }

        if (!IsTownhall(buildingId))
        {
            if (BuildingTownhallComparison(_buildingRepo.GetBuildingById(buildingId)))
            {
                var response = new ErrorResponse(413, "Townhall", "Building cannot have a higher level than the townhall");

                return response;
            }
        }
        if (_buildingRepo.CheckLevel(buildingId))
        {
            var response = new ErrorResponse(403, "BuildingLevel", "You are already max level");

            return response;
        }
        
        if (CheckUpgradeFunds(_buildingRepo.GetKingdomIdFromBuildingId(buildingId), buildingId))
        {
            var response = new ErrorResponse(402, "Resources", "You dont have enough gold or food");

            return response;
        }
        if (IsTownhall(buildingId))
        {
            
            _buildingRepo.UpgradeBuilding(buildingId);
            var building = _buildingRepo.GetBuildingById(buildingId);
            
            if (!_repo.Save())
            {
                var error = new SaveChangesErrorResponse();
                return error;
            }
            var response = new UpgradeBuildingResponse();
            return response;
        }
        else
        {
            _buildingRepo.UpgradeBuilding(buildingId);
            UpgradeKingdomStorageCapacity(_buildingRepo.GetKingdomIdFromBuildingId(buildingId));
            
            if (!_repo.Save())
            {
                var error = new SaveChangesErrorResponse();
                return error;
            }
            
            var response = new UpgradeBuildingResponse();
            return response;
        }
    }

    public IResponse ShowConstructionOptions(int kingdomId, int playerId)
    {
        if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, kingdomId))
        {
            var response = new ErrorResponse(401, "Authorization", "This kingdom isn't yours.");
    
            return response;
        }
        var building =_buildingRepo.GetAllKingdomBuildings(kingdomId);
        
        var farmAmount = building.Count(b => b.BuildingType.Name.ToLower() == _gameConfig.GetBuildingTypeName(0).ToLower());
        var mineAmount = building.Count(b => b.BuildingType.Name.ToLower() == _gameConfig.GetBuildingTypeName(1).ToLower());
    
        var farm = new ConstructionOptionsDTO(_gameConfig.GetBuildingTypeName(0), _gameConfig.GetMaxAmountFarms(), _gameConfig.GetMaxAmountFarms() - farmAmount);
        var mine = new ConstructionOptionsDTO(_gameConfig.GetBuildingTypeName(1), _gameConfig.GetMaxAmountMines(), _gameConfig.GetMaxAmountMines() - mineAmount);
    
        var ConstructionOptions = new List<ConstructionOptionsDTO>()
        {
            farm,
            mine
        };
    
        return new ConstructionOptionsResponse(ConstructionOptions);
    }

    public List<UpgradeOptionsDTO> GetBuildingUpgradeDTOs(int kingdomId)
    {
        var townHallLevel = _buildingRepo.CheckTownHallLevel(kingdomId);
        var buildings = _buildingRepo.GetAllKingdomBuildings(kingdomId)
            .Where(b => b.BuildingType.Level < townHallLevel)
            .ToList();

        var townhall = _buildingRepo.GetTownhall(kingdomId);
        var upgradeOptionsList = new List<UpgradeOptionsDTO>();
        var upgradeOptionTownhall = new UpgradeOptionsDTO(townhall.Id, townhall.BuildingType.Name, townHallLevel,
            _buildingTypeRepo.GetTownhallMaxLevel(),
            _buildingTypeRepo.GetUpgradeCost(townhall.BuildingTypeId));
        upgradeOptionsList.Add(upgradeOptionTownhall);
        
        foreach (var b in buildings)
        {
            if (_buildingTypeRepo.GetUpgradeCost(b.BuildingTypeId) != 0)
            {
                var upgradeOption = new UpgradeOptionsDTO(b.Id, b.BuildingType.Name, b.BuildingType.Level,
                    townHallLevel, _buildingTypeRepo.GetUpgradeCost(b.BuildingTypeId));
                upgradeOptionsList.Add(upgradeOption);
            }
        }
        return upgradeOptionsList;
    }
    public IResponse ShowAvailableUpgrades(int kingdomId, int playerId)
    {
        if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, kingdomId))
        {
            var response = new ErrorResponse(401, "Authorization", "This kingdom isnt yours");
    
            return response;
        }

        var upgradeOptionList = GetBuildingUpgradeDTOs(kingdomId);

        if (upgradeOptionList.Count == 0)
        {
            var response = new ErrorResponse(404, "Upgrade", "No upgradeable buildings were found");
    
            return response;
        }
        else
        {
            var response = new ShowAvailableUpgradesResponse(upgradeOptionList);
            return response;
        }
        
    }

    public List<UnderConstructionDTO> GetUnderConstructionDTOs(int kingdomId)
    {
        var buildings = _buildingRepo.GetAllBuildingsUnderConstruction(kingdomId);
        var buildingsUnderConstructionDTOs = new List<UnderConstructionDTO>();
        
        foreach (var buc in buildings)
        {
            var underConstruction = new UnderConstructionDTO(buc.Id, buc.BuildingType.Name, buc.BuildCompletedAt);
            buildingsUnderConstructionDTOs.Add(underConstruction);
        }

        return buildingsUnderConstructionDTOs;
    }
    

    public IResponse ShowBuildingsUnderConstruction(int kingdomId, int playerId)
    {
        if (!_kingdomRepo.CheckIfPlayerIsOwner(playerId, kingdomId))
        {
            var response = new ErrorResponse(401, "Authorization", "This kingdom isnt yours");
    
            return response;
        }
        else
        {
            var response = new ShowBuildingsUnderConstructionResponse(GetUnderConstructionDTOs(kingdomId));

            return response;
        }
    }

}