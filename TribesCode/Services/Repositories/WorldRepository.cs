using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.APIRequests.WorldRest;
using dusicyon_midnight_tribes_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public class WorldRepository : IWorldRepository
    {
        private readonly IContext _context;

        public WorldRepository(IContext context)
        {
            _context = context;
        }

        public void CreateWorld(World world)
        {
            _context.Worlds.Add(world);
        }
        
        public List<World> GetAllWorlds()
        {
            return _context
                .Worlds
                .Include(w => w.Kingdoms)
                .ToList();
        }

        public World GetWorldById(int worldId)
        {
            return _context
                .Worlds
                .Include(w => w.Kingdoms)
                .FirstOrDefault(w => w.Id == worldId);
        }

        public bool CheckWorldNameExist(string worldName)
        {
            return _context.Worlds.Any(w => w.Name == worldName);
        }

        public bool CheckWorldNameIsValid(string worldName)
        {
            return worldName.Length > 5 && worldName.Length < 20;
        }

        public List<string> GetAllPlayerNamesInTheWorldById(int worldId)
        {
            var playerNames = _context.PlayerWorlds
                .Include(p => p.Player)
                .Where(w => w.WorldId == worldId)
                .Select(pn => pn.Player.UserName)
                .ToList();
            return playerNames;
        }
        
    }
}
