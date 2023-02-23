using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IContext _context;

        public PlayerRepository(IContext context) 
        {
            _context = context;
        }

        public Player GetPlayerById(int id)
        {
            return _context.Players
                .FirstOrDefault(p => p.Id == id);
        }

        public Player GetPlayerByName(string name)
        {
            return _context.Players
                .FirstOrDefault(p => p.UserName == name);
        }

        public Player GetOnlyVerifiedPlayerByEmail(string email)
        {
            return _context.Players
                .FirstOrDefault(p => p.Email == email && p.VerifiedAt != null);
        }

        public bool CheckPlayerExistsByName(string name)
        {
            return _context.Players
                .Any(p => p.UserName == name);
        }

        public bool CheckEmailExists(string email)
        {
            return _context.Players
                .Any(p => p.Email == email);
        }

        public bool CheckPlayerIsVerified(string email)
        {
            return _context.Players
                .Any(p => p.Email == email && p.VerifiedAt != null);
        }

        public List<Player> GetAllPlayers()
        {
            return _context.Players
                .ToList();
        }

        public void AddPlayer(Player player)
        {
            _context.Players
                .Add(player);
        }

        public void UpdatePlayer(Player player)
        {
            _context.Players
                .Update(player);
        }
    }
}