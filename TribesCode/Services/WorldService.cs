using AutoMapper;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Worlds;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using dusicyon_midnight_tribes_backend.Services.Repositories;
using dusicyon_midnight_tribes_backend.Models.APIRequests.WorldRest;
using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services
{
    public class WorldService : IWorldService
    {
        private readonly IGenericRepository _repo;
        private readonly IWorldRepository _worldRepo;
        private readonly IPlayerRepository _playerRepo;
        private readonly IMapper _mapper;
        
        public WorldService(IGenericRepository repo, IWorldRepository worldRepo, IMapper mapper, IPlayerRepository playerRepo)
        {
            _repo = repo;
            _worldRepo = worldRepo;
            _playerRepo = playerRepo;
            _mapper = mapper;
        }

        public IResponse CreateWorld(CreateWorldRequest createWorldRequest, int playerId)
        {
            if (_worldRepo.CheckWorldNameExist(createWorldRequest.Name))
            {
                var response = new ErrorResponse(400, "", "This world Name already exist");
                return response;
            }
            
            var world = new World(createWorldRequest.Name);
            var player = _playerRepo.GetPlayerById(playerId);
            world.PlayerWorlds
                .Add(new PlayerWorld()
                {
                    PlayerId = playerId,
                    WorldId = world.Id,
                    Player = player,
                    World = world,
                });


            _worldRepo.CreateWorld(world);
            
            if (!_repo.Save())
            {
                return new SaveChangesErrorResponse();
            }
            else
            {
                var worldDTO = _mapper.Map<WorldDTO>(world);
                worldDTO.PlayerNames = _worldRepo.GetAllPlayerNamesInTheWorldById(worldDTO.Id);
                var response = new CreateWorldResponse()
                {
                    World = worldDTO
                };
                return response;
            }
        }

        public IResponse GetAllWorlds()
        {
            var resultSource = _worldRepo.GetAllWorlds();

            if (resultSource == null)
            {
                var response = new ErrorResponse(500, "", "Unknown internal server error.");
                return response;
            }
            else
            {
                var worldDTOs = resultSource
                                    .Select(w => _mapper.Map<WorldDTO>(w))
                                    .ToList();
                foreach (var worldDTO in worldDTOs)
                {
                    worldDTO.PlayerNames = _worldRepo.GetAllPlayerNamesInTheWorldById(worldDTO.Id);
                }
                var response = new GetAllWorldsResponse()
                {
                    WorldDTOs = worldDTOs
                };
                return response;
            }
        }

        public IResponse GetWorldById(int worldId)
        {
            if (worldId <= 0)
            {
                var response = new ErrorResponse(400, "", "Id must be a positive integer and not zero.");
                return response;
            }
            var resultSource = _worldRepo.GetWorldById(worldId);

            if (resultSource == null)
            {
                var response = new ErrorResponse(404, "", "This world no exist");
                return response;
            }
            else
            {
                var worldDTO = _mapper.Map<WorldDTO>(resultSource);
                worldDTO.PlayerNames = _worldRepo.GetAllPlayerNamesInTheWorldById(worldId);

                var response = new GetWorldByIdResponse()
                {
                    World = worldDTO
                };
                return response;
            }
        }
    }
}
