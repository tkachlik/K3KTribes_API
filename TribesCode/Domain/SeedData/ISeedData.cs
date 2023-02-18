using dusicyon_midnight_tribes_backend.Models.Entities;

namespace dusicyon_midnight_tribes_backend.Domain.SeedData
{
    public interface ISeedData
    {
        ///<summary>
        ///Seed to DB necessary preliminary data. Data are retrived from CSV file.
        ///CsvHelper needed.
        ///Due to seeding database Package Manager console is used, 
        ///the path to target file is from root folder of project
        ///in out case it Domain\SeedData\nameOfFile.csv
        ///</summary>
        ///<returns> Generic list of BuildingCosts </returns>
        List<BuildingCost> SeedBuildingCost();

        ///<summary>
        ///Seed to DB necessary preliminary data.
        ///</summary>
        ///<returns> Generic list of BuildingTypes </returns>
        List<BuildingType> SeedBuildingType();

        ///<summary>
        ///Seed to DB necessary preliminary data.
        ///</summary>
        ///<returns> Generic list of ResourceType </returns>
        List<ResourceType> SeedResourceType();

        ///<summary>
        ///Seed to DB necessary preliminary data. Data are retrived from CSV file.
        ///CsvHelper needed.
        ///Due to seeding database Package Manager console is used, 
        ///the path to target file is from root folder of project
        ///in out case it Domain\SeedData\nameOfFile.csv
        ///</summary>
        ///<returns> Generic list of ProductionOptions </returns>
        List<ProductionOption> SeedProductionOption();

        ///<summary>
        ///Seed to DB with custom test data (primarily for ease of testing in dev env and demos).
        ///</summary>
        ///<returns> Generic list of Worlds </returns>
        List<World> SeedWorld();

        ///<summary>
        ///Seed to DB with custom test data (primarily for ease of testing in dev env and demos).
        ///</summary>
        ///<returns> Generic list of Players </returns>
        List<Player> SeedPlayer();

        ///<summary>
        ///Seed to DB with custom test data (primarily for ease of testing in dev env and demos).
        ///</summary>
        ///<returns> Generic list of Resource </returns>
        List<Resource> SeedResource();

        ///<summary>
        ///Seed to DB with custom test data (primarily for ease of testing in dev env and demos).
        ///</summary>
        ///<returns> Generic list of Buildings </returns>
        List<Building> SeedBuilding();

        ///<summary>
        ///Seed to DB with custom test data (primarily for ease of testing in dev env and demos).
        ///</summary>
        ///<returns> Generic list of Kingdoms </returns>
        List<Kingdom> SeedKingdom();

        ///<summary>
        ///Seed to DB with custom test data (primarily for ease of testing in dev env and demos).
        ///</summary>
        ///<returns> Generic list of PlayerWorld </returns>
        List<PlayerWorld> SeedPlayerWorld();

        ///<summary>
        ///Seed to DB with custom test data (primarily for ease of testing in dev env and demos).
        ///</summary>
        ///<returns> Generic list of Email Verifications </returns>
        List<EmailVerification> SeedEmailVerifications();

        ///<summary>
        ///Seed to DB with custom test data (primarily for ease of testing in dev env and demos).
        ///</summary>
        ///<returns> Generic list of Productions </returns>
        List<Production> SeedProductions();
    }
}