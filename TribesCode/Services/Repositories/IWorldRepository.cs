using dusicyon_midnight_tribes_backend.Models.APIRequests.WorldRest;
using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public interface IWorldRepository
    {
        void CreateWorld(World world);
        List<World> GetAllWorlds();
        World GetWorldById(int worldId);
        bool CheckWorldNameExist(string worldName);
        bool CheckWorldNameIsValid(string worldName);
        List<string> GetAllPlayerNamesInTheWorldById(int worldId);
    }
}
