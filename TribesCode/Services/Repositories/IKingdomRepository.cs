using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Services.Repositories;

public interface IKingdomRepository
{
    public Kingdom GetKingdomById(int kingdomId);
    List<Kingdom> GetAllKingdoms();
    void AddKingdom(Kingdom kingdom);
    int GetKingdomIdByBuildingId(int buildingId);
    List<Kingdom> GetAllCurrentPlayersKingdoms(int playerId);
    bool CheckIfKingdomExistsById(int kingdomId);
    bool CheckIfPlayerIsOwner(int playerId, int kingdomId);
}
