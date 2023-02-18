using BCrypt.Net;
using dusicyon_midnight_tribes_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using dusicyon_midnight_tribes_backend.Domain.SeedData;

namespace dusicyon_midnight_tribes_backend.Contexts
{

    // When copying to TestDbContext, remember to change the name MANUALLY instead of IDE refactor (CTRL+RR)!!

    //          *** SEED DB in Development or Test enviroment ***
    // When enviroment is Test or Development and developer needs to change parametrs for seeding DB all these 
    // parameters are placed inside -> CheckIsEnvDevOrTestAndSetFieldIfTrue() 

    public class AppDbContext : DbContext, IContext
    {
        private readonly ISeedData _seedData;

        public DbSet<Army> Armies { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingCost> BuildingCosts { get; set; }
        public DbSet<BuildingType> BuildingTypes { get; set; }
        public DbSet<Kingdom> Kingdoms { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<ForgottenPassword> ForgottenPasswords { get; set; }
        public DbSet<PlayerWorld> PlayerWorlds { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductionOption> ProductionOptions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitCost> UnitsCost { get; set; }
        public DbSet<UnitType> UnitsType { get; set; }
        public DbSet<World> Worlds { get; set; }

        // When copying to TestDbContext, remember to change the name MANUALLY instead of IDE refactor (CTRL+RR) !!
        // Remember to change the name also in the constructor argument DbContextOptions<HERE> !!
        public AppDbContext(DbContextOptions<AppDbContext> options, ISeedData seedData) : base(options)
        {
            _seedData = seedData;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerWorld>().HasKey(sc => new { sc.PlayerId, sc.WorldId });

            modelBuilder.Entity<PlayerWorld>()
                        .HasOne<Player>(pw => pw.Player)
                        .WithMany(p => p.PlayerWorlds)
                        .HasForeignKey(pw => pw.PlayerId);

            modelBuilder.Entity<PlayerWorld>()
                        .HasOne<World>(pw => pw.World)
                        .WithMany(w => w.PlayerWorlds)
                        .HasForeignKey(pw => pw.WorldId);

            modelBuilder.Entity<Production>()
                        .HasOne(p => p.Kingdom)
                        .WithMany(k => k.Productions)
                        .HasForeignKey(p => p.KingdomId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Production>()
                        .HasOne(p => p.ProductionOption)
                        .WithMany(po => po.Productions)
                        .HasForeignKey(p => p.ProductionOptionID)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UnitType>()
                        .HasOne(ut => ut.UnitCost)
                        .WithOne(uc => uc.UnitType)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UnitCost>()
                        .HasOne(p => p.ResourceType)
                        .WithMany(r => r.UnitCosts)
                        .HasForeignKey(p => p.ResourceTypeId)
                        .OnDelete(DeleteBehavior.NoAction);


            // DB SEEDING WITH NECESSARY PRELIMINARY DATA

            modelBuilder.Entity<BuildingCost>()
                .HasData(_seedData.SeedBuildingCost());

            modelBuilder.Entity<BuildingType>()
                .HasData(_seedData.SeedBuildingType());

            modelBuilder.Entity<ResourceType>()
                .HasData(_seedData.SeedResourceType());

            modelBuilder.Entity<ProductionOption>()
                .HasData(_seedData.SeedProductionOption());

            //// DB SEEDING WITH CUSTOM TEST DATA

            if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test") ||
                (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"))
            {
                modelBuilder.Entity<World>()
                    .HasData(_seedData.SeedWorld());

                modelBuilder.Entity<Player>()
                    .HasData(_seedData.SeedPlayer());

                modelBuilder.Entity<EmailVerification>()
                    .HasData(_seedData.SeedEmailVerifications());

                modelBuilder.Entity<Resource>()
                    .HasData(_seedData.SeedResource());

                modelBuilder.Entity<Building>()
                    .HasData(_seedData.SeedBuilding());

                modelBuilder.Entity<Kingdom>()
                    .HasData(_seedData.SeedKingdom());

                modelBuilder.Entity<PlayerWorld>()
                    .HasData(_seedData.SeedPlayerWorld());

                modelBuilder.Entity<Production>()
                    .HasData(_seedData.SeedProductions());
            }
        }
    }
}