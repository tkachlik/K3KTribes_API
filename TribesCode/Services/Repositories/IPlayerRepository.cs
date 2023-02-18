using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public interface IPlayerRepository
    {
        void AddPlayer(Player player);
        bool CheckEmailExists(string email);
        bool CheckPlayerIsVerified(string email);
        bool CheckPlayerExistsByName(string name);
        List<Player> GetAllPlayers();
        Player GetPlayerById(int id);
        Player GetPlayerByName(string name);
        Player GetOnlyVerifiedPlayerByEmail(string email);
        void UpdatePlayer(Player player);
    }
}