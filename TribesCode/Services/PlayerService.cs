using AutoMapper;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Players;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using dusicyon_midnight_tribes_backend.Services.Repositories;

namespace dusicyon_midnight_tribes_backend.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repo;
        private readonly IMapper _mapper;

        public PlayerService(IPlayerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IResponse GetAllPlayers()
        {
            var players = _repo.GetAllPlayers();

            if (players == null)
            {
                return new ErrorResponse(400, "", "Unknown error has occurred.");
            }

            var playerDTOs = players
                .Select(p => _mapper.Map<PlayerDTO>(p))
                .ToList();

            return new GetAllPlayersResponse(playerDTOs);
        }

        public IResponse GetPlayerByID(int playerId)
        {
            if (playerId < 1)
            {
                return new ErrorResponse(400, "id", "Must be a positive integer.");
            }

            var player = _repo.GetPlayerById(playerId);

            if (player == null)
            {
                return new ErrorResponse(404, "id", "Player not found.");
            }

            var playerDTO = _mapper.Map<PlayerDTO>(player);

            return new GetPlayerByIdResponse(playerDTO);
        }
    }
}