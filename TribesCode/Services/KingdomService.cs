using AutoMapper;
using dusicyon_midnight_tribes_backend.Domain.GameConfig;
using dusicyon_midnight_tribes_backend.Models.APIRequests.Kingdoms;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Kingdoms;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using dusicyon_midnight_tribes_backend.Services.Repositories;


namespace dusicyon_midnight_tribes_backend.Services;

public class KingdomService : IKingdomService
{
    private readonly IMapper _mapper;
    private readonly IKingdomRepository _kingdomRepo;
    private readonly IGenericRepository _repo;
    private readonly IWorldRepository _worldRepo;
    private readonly IBuildingTypeRepository _buildingTypeRepository;
    private readonly IGameConfig _gameConfig;
    public KingdomService(IMapper mapper, 
                            IKingdomRepository kingdomRepo, 
                            IGenericRepository repository,
                            IWorldRepository world,
                            IBuildingTypeRepository buildingTypeRepository,
                            IGameConfig gameConfig)
    {
        _mapper = mapper;
        _kingdomRepo = kingdomRepo;
        _repo = repository;
        _worldRepo = world;
        _buildingTypeRepository = buildingTypeRepository;
        _gameConfig = gameConfig;
    }

    public IResponse GetKingdomByID(int kingdomId)
    {
        var kingdom = _kingdomRepo.GetKingdomById(kingdomId);

        if (kingdom == null)
        {
            return new ErrorResponse(404, "KingdomID", "Kingdom not found.");
        }

        var kingdomDTO = _mapper.Map<KingdomStatsDTO>(kingdom);

        return new GetKingdomResponse(kingdomDTO);
    }
    

    public IResponse GetAllKingdoms()
    {
        var resultSource = _kingdomRepo.GetAllKingdoms();
        var kingdomDtos = new List<KingdomDTO>();

        if (resultSource == null)
        {
            var response = new ErrorResponse(500, "", "Unknown internal server error.");
            return response;
        }
        else
        {
            foreach (var a in resultSource)
            {
                var x = _mapper.Map<KingdomDTO>(a);
                kingdomDtos.Add(x);
            }

            var response = new GetAllKingdomsResponse()
            {
                KingdomDTOs = kingdomDtos
            };
            return response;
        }
    }

    private bool CheckCoordinates(int x, int y, int worldId)
    {
        var kingdoms = _kingdomRepo.GetAllKingdoms()
            .FindAll(k => k.WorldId == worldId);

        foreach (var k in kingdoms)
        {
            if (x == k.Coordinate_X && y == k.Coordinate_Y)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckKingdomsAmount(int playerId, int worldId)
    {
        var kingdoms = _kingdomRepo.GetAllKingdoms()
            .FindAll(k => k.PlayerId == playerId && k.WorldId == worldId);

        if (kingdoms.Count != 0)
        {
            return true;
        }

        return false;
    }

    private bool DoesNameExist(string name, int worldId)
    {
        var kingdoms = _kingdomRepo.GetAllKingdoms().FindAll(k => k.WorldId == worldId && k.Name == name);
        if (kingdoms.Count > 0)
        {
            return true;
        }

        return false;
    }
    
    public bool IsPlayersKingdom(int playerId, int kingdomId)
    {
        var kingdoms = _kingdomRepo.GetAllKingdoms()
            .Any(b => b.Id == kingdomId && b.PlayerId == playerId); //
        
        return kingdoms;
    }

    public IResponse Create(CreateKingdomRequest request, int playerId)
    {
        if (_worldRepo.GetWorldById(request.WorldId) == null)
        {
            var response = new ErrorResponse(404, "World", "Does not exist");

            return response;
        }

        if (CheckCoordinates(request.Coordinate_x, request.Coordinate_y, request.WorldId))
        {
            var response = new ErrorResponse(409, "Coordinates", "Are already occupied");

            return response;
        }

        if (DoesNameExist(request.Name, request.WorldId))
        {
            var response = new ErrorResponse(409, "Name", "Already Exists");

            return response;
        }
        
        if (CheckKingdomsAmount(playerId, request.WorldId))
        {
            var response = new ErrorResponse(409, "Kingdoms", "You already have a kingdom in this world.");

            return response;
        }

        else
        {
            var newKingdom = new Kingdom(request.Name, request.WorldId, playerId, request.Coordinate_x,
                request.Coordinate_y);

            newKingdom.Buildings.Add(new Building(_buildingTypeRepository.GetDefaultBuildingTypeIdByName(_gameConfig.GetTownhallName()), true)); //default Townhall
            newKingdom.Buildings.Add(new Building(_buildingTypeRepository.GetDefaultBuildingTypeIdByName(_gameConfig.GetBuildingTypeName(0)), true)); //default Farm
            newKingdom.MaxStorage = _gameConfig.GetMaxStorageStep()
                                    * _buildingTypeRepository.GetDefaultMaxStorageMultiplicatorByLowestSeedLevel();

            _kingdomRepo.AddKingdom(newKingdom);

            if (!_repo.Save())
            {
                return new SaveChangesErrorResponse();
            }

            var kingdomCreatedDTO = _mapper.Map<MyKingdomDTO>(newKingdom);

            return new CreateKingdomResponse(kingdomCreatedDTO);
        }
    }

    public IResponse GetLoggedInPlayersKingdoms(int playerId)
    {
        var kingdoms = _kingdomRepo.GetAllCurrentPlayersKingdoms(playerId);

        if (kingdoms == null)
        {
            return new ErrorResponse(500, "", "Internal server error");
        }

        var result = kingdoms.Select(k => _mapper.Map<MyKingdomDTO>(k)).ToList();

        return new GetAllMyKingdomsResponse(result);
    }
}