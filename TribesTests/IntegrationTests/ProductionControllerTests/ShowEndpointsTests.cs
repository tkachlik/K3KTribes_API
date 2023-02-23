using dusicyon_midnight_tribes_backend.Models.APIRequests.Productions;
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
            _showAllProductionOptionsUri = "/api/productions/show-available-production-options-of-kingdom";
            _showAllUncollectedProductionsUri = "/api/productions/show-all-uncollected-productions";
        }

        [Theory]
        [MemberData(nameof(ShowAvailableProductionOptions_OkTests_Data))]
        public async Task ShowAvailableProductionOptions_OkTests(int playerId, ShowAvailableProductionOptionsRequest request, ShowAvailableProductionOptionsResponse expectedResponse)
        {
            string bearerToken = $"Bearer {_testTokenService.GenerateTestToken(playerId)}";
            _client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var actualResult = await _client.PostAsJsonAsync(_showAllProductionOptionsUri, request);
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

                    new ShowAvailableProductionOptionsRequest()
                    {
                        KingdomId = 1
                    },

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

                    new ShowAvailableProductionOptionsRequest()
                    {
                        KingdomId = 2
                    },

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
        public async Task ShowAvailableProductionOptions_DataValidationErrorTests(ShowAvailableProductionOptionsRequest request, ValidationResultModel expectedResponse)
        {
            _client.DefaultRequestHeaders.Add("Authorization", _bearerTokenForPlayer1);

            var actualResult = await _client.PostAsJsonAsync(_showAllProductionOptionsUri, request);
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
                    new ShowAvailableProductionOptionsRequest()
                    {
                        KingdomId = 0
                    },

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
                    new ShowAvailableProductionOptionsRequest()
                    {
                        KingdomId = -5
                    },

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
        public async Task ShowAllProductionOptions_DatabaseError_Tests(int playerId, ShowAvailableProductionOptionsRequest request, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            string bearerToken = $"Bearer {_testTokenService.GenerateTestToken(playerId)}";
            _client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var actualResult = await _client.PostAsJsonAsync(_showAllProductionOptionsUri, request);
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

                    new ShowAvailableProductionOptionsRequest()
                    {
                        KingdomId = 1000,
                    },

                    new ErrorResponse(404, "KingdomId", "No such kingdom found."),

                    HttpStatusCode.NotFound
                },

                new object[]
                {
                    2,

                    new ShowAvailableProductionOptionsRequest()
                    {
                        KingdomId = 1,
                    },

                    new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to view its Production Options!"),

                    HttpStatusCode.Unauthorized
                }
            };

        [Theory]
        [MemberData(nameof(ShowAllUncollectedProductions_OkTests_Data))]
        public async Task ShowAllUncollectedProductions_OkTests(int playerId, ShowAllUncollectedProductionsRequest request, ShowAllUncollectedProductionsResponse expectedResponse)
        {
            string bearerToken = $"Bearer {_testTokenService.GenerateTestToken(playerId)}";
            _client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var actualResult = await _client.PostAsJsonAsync(_showAllUncollectedProductionsUri, request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ShowAllUncollectedProductionsResponse>();

            var expectedStatusCode = HttpStatusCode.OK;

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowAllUncollectedProductions_OkTests_Data =>
            new List<object[]>
            {
                new object[]
                {
                    1,

                    new ShowAllUncollectedProductionsRequest()
                    {
                        KingdomId = 1
                    },

                    new ShowAllUncollectedProductionsResponse
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
                    1,

                    new ShowAllUncollectedProductionsRequest()
                    {
                        KingdomId = 0001
                    },

                    new ShowAllUncollectedProductionsResponse
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

                    new ShowAllUncollectedProductionsRequest()
                    {
                        KingdomId = 2
                    },

                    new ShowAllUncollectedProductionsResponse
                    (
                        new List<ProductionDTO>()
                    )
                },

                new object[]
                {
                    2,

                    new ShowAllUncollectedProductionsRequest()
                    {
                        KingdomId = 02
                    },

                    new ShowAllUncollectedProductionsResponse
                    (
                        new List<ProductionDTO>()
                    )
                }
            };

        [Theory]
        [MemberData(nameof(ShowAllUncollectedProductions_ValidationErrorTests_Data))]
        public async Task ShowAllUncollectedProductions_ValidationErrorTests(ShowAllUncollectedProductionsRequest request, ValidationResultModel expectedResponse)
        {
            _client.DefaultRequestHeaders.Add("Authorization", _bearerTokenForPlayer1);

            var actualResult = await _client.PostAsJsonAsync(_showAllUncollectedProductionsUri, request);
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
                    new ShowAllUncollectedProductionsRequest()
                    {
                        KingdomId = 0
                    },

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
                    new ShowAllUncollectedProductionsRequest()
                    {
                        KingdomId = -5
                    },

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
        public async Task ShowAllUncollectedProductions_DatabaseErrorTests(ShowAllUncollectedProductionsRequest request, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            _client.DefaultRequestHeaders.Add("Authorization", _bearerTokenForPlayer1);

            var actualResult = await _client.PostAsJsonAsync(_showAllUncollectedProductionsUri, request);
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
                    new ShowAllUncollectedProductionsRequest()
                    {
                        KingdomId = 1000
                    },

                    new ErrorResponse(404, "KingdomId", "No such kingdom exists."),

                    HttpStatusCode.NotFound
                },

                new object[]
                {
                    new ShowAllUncollectedProductionsRequest()
                    {
                        KingdomId = 2
                    },

                    new ErrorResponse(401, "KingdomId", "This kingdom does not belong to you. You are not authorized to collect its productions!"),

                    HttpStatusCode.Unauthorized
                }
                
            };
    }
}