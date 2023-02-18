using dusicyon_midnight_tribes_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace dusicyon_midnight_tribes_backend.Contexts
{
    public interface IContext
    {
        DbSet<Army> Armies { get; set; }
        DbSet<BuildingCost> BuildingCosts { get; set; }
        DbSet<Building> Buildings { get; set; }
        DbSet<BuildingType> BuildingTypes { get; set; }
        DbSet<EmailVerification> EmailVerifications { get; set; }
        DbSet<ForgottenPassword> ForgottenPasswords { get; set; }
        DbSet<Kingdom> Kingdoms { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<PlayerWorld> PlayerWorlds { get; set; }
        DbSet<ProductionOption> ProductionOptions { get; set; }
        DbSet<Production> Productions { get; set; }
        DbSet<Resource> Resources { get; set; }
        DbSet<ResourceType> ResourceTypes { get; set; }
        DbSet<Unit> Units { get; set; }
        DbSet<UnitCost> UnitsCost { get; set; }
        DbSet<UnitType> UnitsType { get; set; }
        DbSet<World> Worlds { get; set; }

        int SaveChanges();
    }
}