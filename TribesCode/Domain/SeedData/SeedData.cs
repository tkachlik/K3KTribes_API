using CsvHelper.Configuration;
using CsvHelper;
using dusicyon_midnight_tribes_backend.Domain.SeedModel;
using dusicyon_midnight_tribes_backend.Models.Entities;
using System.Globalization;
using dusicyon_midnight_tribes_backend.Domain.GameConfig;
using BCrypt.Net;
using System.Runtime.InteropServices;

namespace dusicyon_midnight_tribes_backend.Domain.SeedData
{
    public class SeedData : ISeedData
    {
        private readonly int _startLevelOfSeedData = 1;
        private readonly int _endLevelOfSeedData = 5;
        private readonly int _timeAccelerator = 50;

        private readonly IGameConfig _gameConfig;

        public SeedData(IGameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            CheckIsEnvDevOrTestAndSetFieldIfTrue();
        }

        private void CheckIsEnvDevOrTestAndSetFieldIfTrue() 
        {
            if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test") ||
                (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"))
            {
                _gameConfig.SetStartLevelOfSeedData(_startLevelOfSeedData);
                _gameConfig.SetEndLevelOfSeedData(_endLevelOfSeedData);
                _gameConfig.SetTimeAccelerator(_timeAccelerator);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                { 
                    _gameConfig.SetResourcePath(@"Domain/SeedData/");
                }
                else
                {
                    _gameConfig.SetResourcePath(@"Domain\SeedData\");
                }
            }
            else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                _gameConfig.SetStartLevelOfSeedData(1);
                _gameConfig.SetEndLevelOfSeedData(20);
                _gameConfig.SetTimeAccelerator(1);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    _gameConfig.SetResourcePath(@"Domain/SeedData/");
                }
                else
                {
                    _gameConfig.SetResourcePath(@"Domain\SeedData\");
                }
            }
        }
        
        public List<BuildingCost> SeedBuildingCost()
        {
            List<BuildingCost> buildingCosts = new List<BuildingCost>();

        int id = 1;
            for (int buildNameIndex = 0; buildNameIndex < _gameConfig.GetAllBuildingTypeNames().Count; buildNameIndex++)
            {

                string filePath = @$"{_gameConfig.GetResourcePath()}{_gameConfig.GetBuildingTypeName(buildNameIndex)}.csv";
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };
                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, config))
                    {
                        if (_gameConfig.GetAllBuildingTypeNames()[buildNameIndex] == _gameConfig.GetTownhallName())
                        {
                            var records = csv.GetRecords<CSV_Townhall_SeedModel>().ToList();
                            for (int i = _gameConfig.GetStartLevelOfSeedData(); i <= _gameConfig.GetEndLevelOfSeedData(); i++)
                            {
                                TimeSpan ts = TimeSpan.Parse(records[i - 1].ProdTime);
                                var buildingTime = Convert.ToInt32(ts.TotalSeconds / _gameConfig.GetTimeAccelerator());
                                var buildingCost = new BuildingCost(id, id, buildingTime, records[i - 1].Cost);
                                buildingCosts.Add(buildingCost);
                                id++;
                            }
                        }
                        else
                        {
                            var records = csv.GetRecords<CSV_SeedModel>().ToList();
                            for (int i = _gameConfig.GetStartLevelOfSeedData(); i <= _gameConfig.GetEndLevelOfSeedData(); i++)
                            {
                                TimeSpan ts = TimeSpan.Parse(records[i - 1].ProdTime);
                                var buildingTime = Convert.ToInt32(ts.TotalSeconds / _gameConfig.GetTimeAccelerator());
                                var buildingCost = new BuildingCost(id, id, buildingTime, records[i - 1].Cost);
                                buildingCosts.Add(buildingCost);
                                id++;
                            }
                        }
                    }

                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return buildingCosts;
        }

        public List<BuildingType> SeedBuildingType()
        {
            List<BuildingType> buildingTypes = new List<BuildingType>();

            int id = 1;
            for (int buildNameIndex = 0; buildNameIndex < _gameConfig.GetAllBuildingTypeNames().Count; buildNameIndex++)
            {
                for (int i = _gameConfig.GetStartLevelOfSeedData(); i <= _gameConfig.GetEndLevelOfSeedData(); i++)
                {
                    var tempBuildingType = new BuildingType(id, _gameConfig.GetBuildingTypeName(buildNameIndex), i, id);
                    buildingTypes.Add(tempBuildingType);
                    id++;
                }
            }

            return buildingTypes;
        }

        public List<ResourceType> SeedResourceType()
        {
            List<ResourceType> resourceTypes = new List<ResourceType>();

            for(int i = 0; i<_gameConfig.GetAllResourcetypeNames().Count; i++)
            {
                var resourceType = new ResourceType(i+1, _gameConfig.GetAllResourcetypeNames()[i]);
                resourceTypes.Add(resourceType);
            }

            return resourceTypes;
        }

        public List<ProductionOption> SeedProductionOption()
        {
            List<ProductionOption> productionOptions = new List<ProductionOption>();
            int id = 1;
            for (int buildNameIndex = 0; buildNameIndex < _gameConfig.GetAllBuildingTypeNames().Count; buildNameIndex++)
            {
                if (_gameConfig.GetAllBuildingTypeNames()[buildNameIndex] == _gameConfig.GetTownhallName())
                    { break; }

                string filePath = @$"{_gameConfig.GetResourcePath()}{_gameConfig.GetBuildingTypeName(buildNameIndex)}.csv";
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };
                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<CSV_SeedModel>().ToList();

                        
                        for (int i = _gameConfig.GetStartLevelOfSeedData(); i <= _gameConfig.GetEndLevelOfSeedData(); i++)
                        {
                            var amount = records[i - 1].Production;
                            TimeSpan ts = TimeSpan.Parse(records[i - 1].ProdTime);
                            var prodTime = Convert.ToInt32(ts.TotalSeconds / _gameConfig.GetTimeAccelerator());
                            var resourceTypeId = 0;

                            if (_gameConfig.GetAllBuildingTypeNames()[buildNameIndex] == _gameConfig.GetBuildingTypeName(0))
                            { resourceTypeId = 2; }
                            else
                            { resourceTypeId = 1; }

                            var productionOption = new ProductionOption(id, id, resourceTypeId, null, amount, prodTime);

                            productionOptions.Add(productionOption);
                            id++;
                        }
                    }
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return productionOptions;
        }

        public List<Production> SeedProductions()
        {
            var productions = new List<Production>()
            {
                new Production()
                {
                    Id = 1,
                    StartedAt = new DateTime(2023, 1, 1),
                    CompletedAt = new DateTime(2023, 1, 1, 1, 0, 0),
                    KingdomId = 1,
                    BuildingId = 2,
                    ProductionOptionID = 1
                },

                new Production()
                {
                    Id = 2,
                    Collected = true,
                    StartedAt = new DateTime(2023, 1, 1),
                    CompletedAt = new DateTime(2023, 1, 1, 1, 0, 0),
                    KingdomId = 1,
                    BuildingId = 4,
                    ProductionOptionID = 6
                }
            };

            return productions;
        }

        public List<World> SeedWorld()
        {
            List<World> worlds = new List<World>()
            {
                new World()
                {
                    Id = 1,
                    Name = "Svetozor - test",
                },
                new World ()
                {
                    Id = 2,
                    Name = "Lucerna - test",
                }
             };

            return worlds;
        }

        public List<PlayerWorld> SeedPlayerWorld()
        {
            List<PlayerWorld> playerWorlds = new List<PlayerWorld>()
            {
                new PlayerWorld(){PlayerId = 1,WorldId=1},
                new PlayerWorld(){PlayerId = 2,WorldId=2}
            };

            return playerWorlds;
        }

        public List<Player> SeedPlayer()
        {
            List<Player> players = new List<Player>()
            {   new Player()
                {
                    Id = 1,
                    UserName = "King Richard - test",
                    Email = "test@test.test",
                    PasswordHashed = BCrypt.Net.BCrypt.EnhancedHashPassword("heslo123", hashType: HashType.SHA384),
                    Kingdoms = new List<Kingdom>(),
                    VerifiedAt = new DateTime(2023, 1, 1)
                },
                new Player()
                {
                    Id = 2,
                    UserName = "tkachlik",
                    Email = "tkachlik@gmail.com",
                    PasswordHashed = BCrypt.Net.BCrypt.EnhancedHashPassword("heslo123", hashType: HashType.SHA384),
                    Kingdoms = new List<Kingdom>()
                }
            };

            return players;
        }

        public List<EmailVerification> SeedEmailVerifications()
        {
            var emailVerifications = new List<EmailVerification>()
            {
                new EmailVerification()
                {
                    Id = 1,
                    PlayerId = 1,
                    Token = "dummy JWT",
                    ExpiresAt = DateTime.Now.AddYears(100),
                    CreatedAt = DateTime.Now,
                }
            };

            return emailVerifications;
        }

        public List<Resource> SeedResource()
        {
            List<Resource> resources = new List<Resource>()
            {
                new Resource()
                {
                    Id = 1,
                    Amount = 1000,
                    ResourceTypeId = 1,
                    KingdomId = 1
                },
                new Resource()
                {
                    Id = 2,
                    Amount = 1000,
                    ResourceTypeId = 2,
                    KingdomId = 1
                },
                new Resource() 
                {
                    Id = 3,
                    Amount = 1000,
                    ResourceTypeId = 1,
                    KingdomId = 2
                },
                new Resource()
                {
                    Id = 4,
                    Amount = 1000,
                    ResourceTypeId = 2,
                    KingdomId = 2
                }
            };

            return resources;
        }

        public List<Building> SeedBuilding()
        {
            List<Building> buildings = new List<Building>()
            {
                new Building() // Townhall
                {
                    Id = 1,
                    BuildingTypeId = _endLevelOfSeedData*2+1,
                    BuildStartedAt = DateTime.Now,
                    BuildCompletedAt = DateTime.Now,
                    KingdomId = 1,
                    Productions = new List<Production>()
                },

                new Building() // Farm Level 1
                {
                    Id = 2,
                    BuildingTypeId = _startLevelOfSeedData,
                    BuildStartedAt = DateTime.Now,
                    BuildCompletedAt = DateTime.Now,
                    KingdomId = 1,
                    Productions = new List<Production>()
                },

                new Building() // Farm Level 5
                {
                    Id = 3,
                    BuildingTypeId = _endLevelOfSeedData,
                    BuildStartedAt = DateTime.Now,
                    BuildCompletedAt = DateTime.Now,
                    KingdomId = 1,
                    Productions = new List<Production>()
                },

                new Building() // Mine Level 1
                {
                    Id = 4,
                    BuildingTypeId = _endLevelOfSeedData+1,
                    BuildStartedAt = DateTime.Now,
                    BuildCompletedAt = DateTime.Now,
                    KingdomId = 1,
                    Productions = new List<Production>()
                },

                new Building() // Mine Level 5
                {
                    Id = 5,
                    BuildingTypeId = _endLevelOfSeedData*2,
                    BuildStartedAt = DateTime.Now,
                    BuildCompletedAt = DateTime.Now,
                    KingdomId = 1,
                    Productions = new List<Production>()
                },

                new Building() // Townhall
                {
                    Id = 6,
                    BuildingTypeId = _gameConfig.GetEndLevelOfSeedData() * 2 + 1, 
                    BuildStartedAt = DateTime.Now,
                    BuildCompletedAt = DateTime.Now,
                    KingdomId = 2,
                    Productions = new List<Production>()
                },

                new Building() // Farm Level 1
                {
                    Id = 7,
                    BuildingTypeId = _gameConfig.GetStartLevelOfSeedData(),
                    BuildStartedAt = DateTime.Now,
                    BuildCompletedAt = DateTime.Now,
                    KingdomId = 2,
                    Productions = new List<Production>()
                },
            };

            return buildings;
        }

        public List<Kingdom> SeedKingdom()
        {
            List<Kingdom> kingdoms = new List<Kingdom>()
            {
                new Kingdom
                {
                    Id = 1,
                    Name = "Avalon - test",
                    WorldId = 1,
                    PlayerId = 1,
                    Coordinate_X = 0,
                    Coordinate_Y = 0,
                    MaxStorage = 1200,
                    Armies = new List<Army>(),
                },

                new Kingdom
                {
                    Id = 2,
                    Name = "Matrix - test",
                    WorldId = 2,
                    PlayerId = 2,
                    Coordinate_X = 0,
                    Coordinate_Y = 0,
                    MaxStorage = 1200,
                    Armies = new List<Army>(),
                }
            };

            return kingdoms;
        }
    }
}
