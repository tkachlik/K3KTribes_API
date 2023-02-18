using System.Runtime.CompilerServices;
using System.Xml.Linq;
using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using HostingEnvironmentExtensions = Microsoft.Extensions.Hosting.HostingEnvironmentExtensions;

namespace dusicyon_midnight_tribes_backend.Services.Repositories;

public class KingdomRepository : IKingdomRepository
{
    private readonly IContext _context;
    private readonly IBuildingTypeRepository _buildingTypeRepository;
    public KingdomRepository(IContext context, IBuildingTypeRepository buildingTypeRepository)
    {
        _context = context;
        _buildingTypeRepository = buildingTypeRepository;
    }
    public Kingdom GetKingdomById(int kingdomId)
    {
        return _context.Kingdoms
            .Include(k => k.Player)
            .Include(k => k.World)
            .Include(k => k.Buildings)
                .ThenInclude(b => b.BuildingType)
            .Include(k => k.Resources)
            .FirstOrDefault(p => p.Id == kingdomId);
    }

    public List<Kingdom> GetAllKingdoms()
    {
        return _context.Kingdoms
            .Include(k => k.World)
            .Include(k => k.Player)
            .ToList();
    }

    public void AddKingdom(Kingdom kingdom)
    {
        _context.Kingdoms
            .Add(kingdom);
    }

    public int GetKingdomIdByBuildingId(int buildingId)
    {
        return _context.Buildings
            .FirstOrDefault(b => b.Id == buildingId)
            .KingdomId;
    }

    public List<Kingdom> GetAllCurrentPlayersKingdoms(int playerId)
    {
        return _context.Kingdoms
            .Include(k => k.World)
            .Where(k => k.PlayerId == playerId)
            .ToList();
    }

    public bool CheckIfKingdomExistsById(int kingdomId)
    {
        return _context.Kingdoms
            .Any(k => k.Id == kingdomId);
    }

    public bool CheckIfPlayerIsOwner(int playerId, int kingdomId)
    {
        return _context.Kingdoms
            .FirstOrDefault(k => k.Id == kingdomId)
            .PlayerId.Equals(playerId);
    }
}