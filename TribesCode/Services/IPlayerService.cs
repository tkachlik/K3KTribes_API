using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services
{
    public interface IPlayerService
    {
        IResponse GetAllPlayers();
        IResponse GetPlayerByID(int playerId);
    }
}