using dusicyon_midnight_tribes_backend.Models.APIRequests.WorldRest;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services
{
    public interface IWorldService
    {
        IResponse GetAllWorlds();
        IResponse GetWorldById(int worldId);
        IResponse CreateWorld(CreateWorldRequest world, int playerId);
    }
}
