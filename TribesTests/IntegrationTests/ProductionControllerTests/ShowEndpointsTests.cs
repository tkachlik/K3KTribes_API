using dusicyon_midnight_tribes_backend.Models.APIResponses.Productions;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TribesTests.Helpers;

namespace TribesTests.IntegrationTests.ProductionControllerTests
{
    public class ShowEndpointsTests : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<TestTokenHelper>
    {
        private readonly HttpClient _client;
        private readonly TestTokenHelper _testTokenService;
        private readonly string _bearerTokenForPlayer1;
        private readonly string _showAllProductionOptionsUri;
        private readonly string _showAllUncollectedProductionsUri;

        public ShowEndpointsTests(WebApplicationFactory<Program> factory, TestTokenHelper testTokenService)
        {
            _client = factory.CreateClient();
            _testTokenService = testTokenService;
            _bearerTokenForPlayer1 = $"Bearer {_testTokenService.GenerateTestToken(1)}";
            _showAllProductionOptionsUri = "/api/productions/show-available-production-options";
            _showAllUncollectedProductionsUri = "/api/productions/show-uncollected-productions";
        }

        [Theory]
        [MemberData(nameof(ShowAvailableProductionOptions_OkTests_Data))]
        public async Task ShowAvailableProductionOptions_OkTests(int playerId, int kingdomId, ShowAvailableProductionOptionsResponse expectedResponse)
        {
            string bearerToken = $"Bearer {_testTokenService.GenerateTestToken(playerId)}";
            _client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var actualResult = await _client.GetAsync($"{_showAllProductionOptionsUri}/{kingdomId}");
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ShowAvailableProductionOptionsResponse>();

            var expectedStatusCode = HttpStatusCode.OK;

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowAvailableProductionOptions_OkTests_Data =>
            new List<object[]>
            {
                new object[]
                {
                    1,

                    1,

                    new ShowAvailableProductionOptionsResponse
                    (
                        new List<ProductionOptionsAvailableOnCompletedBuildingsDTO>()
                        {
                            new ProductionOptionsAvailableOnCompletedBuildingsDTO()
                            {
                                BuildingType = "Farm",
                                BuildingLevel = 1,
                                BuildingId = 2,
                                ProductionOptions = new List<ProductionOptionDTO>()
                                {
                                    new ProductionOptionDTO()
                                    {
                                        ResourceType = "Food",
                                        AmountProduced = 6,
                                        ProductionTime = 10,
                                        ProductionOptionId = 1
                                    }
                                }
                            },

                            new ProductionOptionsAvailableOnCompletedBuildingsDTO()
                            {
                                BuildingType = "Farm",
                                BuildingLevel = 5,
                                BuildingId = 3,
                                ProductionOptions = new List<ProductionOptionDTO>()
                                {
                                    new ProductionOptionDTO()
                                    {
                                        ResourceType = "Food",
                                        AmountProduced = 34,
                                        ProductionTime = 19,
                                        ProductionOptionId = 5
                                    }
                                }
                            },

                            new ProductionOptionsAvailableOnCompletedBuildingsDTO()
                            {
                                BuildingType = "Mine",
                                BuildingLevel = 1,
                                BuildingId = 4,
                                ProductionOptions = new List<ProductionOptionDTO>()
                                {
                                    new ProductionOptionDTO()
                                    {
                                        ResourceType = "Gold",
                                        AmountProduced = 5,
                                        ProductionTime = 12,
                                        ProductionOptionId = 6
                                    }
                                }
                            },

                            new ProductionOptionsAvailableOnCompletedBuildingsDTO()
                            {
                                BuildingType = "Mine",
                                BuildingLevel = 5,
                                BuildingId = 5,
                                ProductionOptions = new List<ProductionOptionDTO>()
                                {
                                    new ProductionOptionDTO()
                                    {
                                        ResourceType = "Gold",
                                        AmountProduced = 28,
                                        ProductionTime = 24,
                                        ProductionOptionId = 10
                                    }
                                }
                            },
                        }
                    )
                },

                new object[]
                {
                    2,

                    2,

                    new ShowAvailableProductionOptionsResponse
                    (
                        new List<ProductionOptionsAvailableOnCompletedBuildingsDTO>()
                        {
                            new ProductionOptionsAvailableOnCompletedBuildingsDTO()
                            {
                                BuildingType = "Farm",
                                BuildingLevel = 1,
                                BuildingId = 7,
                                ProductionOptions = new List<ProductionOptionDTO>()
                                {
                                    new ProductionOptionDTO()
                                    {
                                        ResourceType = "Food",
                                        AmountProduced = 6,
                                        ProductionTime = 10,
                                        ProductionOptionId = 1
                                    }
                                }
                            }
                        }
                    )
                }
            };

        [Theory]
        [MemberData(nameof(ShowAvailableProductionOptions_DataValidationErrorTests_Data))]
        public async Task ShowAvailableProductionOptions_DataValidationErrorTests(int kingdomId, ValidationResultModel expectedResponse)
        {
            _client.DefaultRequestHeaders.Add("Authorization", _bearerTokenForPlayer1);

            var actualResult = await _client.GetAsync($"{_showAllProductionOptionsUri}/{kingdomId}");
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ValidationResultModel>();

            var expectedStatusCode = HttpStatusCode.BadRequest;

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowAvailableProductionOptions_DataValidationErrorTests_Data =>
            new List<object[]>
            {
                new object[]
                {
                    0,

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "KingdomId",
                                ErrorMessage = "Must be a positive integer."
                            }
                        }
                    }
                },

                new object[]
                {
                    -5,

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "KingdomId",
                                ErrorMessage = "Must be a positive integer."
                            }
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(ShowAllProductionOptions_DatabaseError_Tests_Data))]
        public async Task ShowAllProductionOptions_DatabaseError_Tests(int playerId, int kingdomId, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            string bearerToken = $"Bearer {_testTokenService.GenerateTestToken(playerId)}";
            _client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var actualResult = await _client.GetAsync($"{_showAllProductionOptionsUri}/{kingdomId}");
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowAllProductionOptions_DatabaseError_Tests_Data =>
            new List<object[]>
            {
                new object[]
                {
                    1,

                    1000,

                    new ErrorResponse(404, "KingdomId", "No such kingdom found."),

                    HttpStatusCode.NotFound
                },

                new object[]
                {
                    2,

                    1,

                    new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to view its Production Options!"),

                    HttpStatusCode.Unauthorized
                }
            };

        [Theory]
        [MemberData(nameof(ShowUncollectedProductions_OkTests_Data))]
        public async Task ShowUncollectedProductions_OkTests(int playerId, int kingdomId, ShowUncollectedProductionsResponse expectedResponse)
        {
            string bearerToken = $"Bearer {_testTokenService.GenerateTestToken(playerId)}";
            _client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var actualResult = await _client.GetAsync($"{_showAllUncollectedProductionsUri}/{kingdomId}");
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ShowUncollectedProductionsResponse>();

            var expectedStatusCode = HttpStatusCode.OK;

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowUncollectedProductions_OkTests_Data =>
            new List<object[]>
            {
                new object[]
                {
                    1,

                    1,

                    new ShowUncollectedProductionsResponse
                    (
                        new List<ProductionDTO>()
                        {
                            new ProductionDTO()
                            {
                                ResourceType = "Food",
                                AmountProduced = 6,
                                IsReadyForCollection = true,
                                ReadyAt = "2023.01.01. 01:00:00",
                                ProductionId = 1
                            }
                        }
                    )
                },

                new object[]
                {
                    2,

                    2,

                    new ShowUncollectedProductionsResponse
                    (
                        new List<ProductionDTO>()
                    )
                },

                new object[]
                {
                    2,

                    02,

                    new ShowUncollectedProductionsResponse
                    (
                        new List<ProductionDTO>()
                    )
                }
            };

        [Theory]
        [MemberData(nameof(ShowAllUncollectedProductions_ValidationErrorTests_Data))]
        public async Task ShowAllUncollectedProductions_ValidationErrorTests(int kingdomId, ValidationResultModel expectedResponse)
        {
            _client.DefaultRequestHeaders.Add("Authorization", _bearerTokenForPlayer1);

            var actualResult = await _client.GetAsync($"{_showAllUncollectedProductionsUri}/{kingdomId}");
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ValidationResultModel>();

            var expectedStatusCode = HttpStatusCode.BadRequest;

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowAllUncollectedProductions_ValidationErrorTests_Data =>
            new List<object[]>
            {
                new object[]
                {
                    0,

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "KingdomId",
                                ErrorMessage = "Must be a positive integer."
                            }
                        }
                    }
                },

                new object[]
                {
                    -5,

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "KingdomId",
                                ErrorMessage = "Must be a positive integer."
                            }
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(ShowAllUncollectedProductions_DatabaseErrorTests_Data))]
        public async Task ShowAllUncollectedProductions_DatabaseErrorTests(int kingdomId, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            _client.DefaultRequestHeaders.Add("Authorization", _bearerTokenForPlayer1);

            var actualResult = await _client.GetAsync($"{_showAllUncollectedProductionsUri}/{kingdomId}");
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowAllUncollectedProductions_DatabaseErrorTests_Data =>
            new List<object[]>
            {
                new object[]
                {
                    1000,

                    new ErrorResponse(404, "KingdomId", "No such kingdom exists."),

                    HttpStatusCode.NotFound
                },

                new object[]
                {
                    2,

                    new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to collect its productions!"),

                    HttpStatusCode.Unauthorized
                }
                
            };
    }
}