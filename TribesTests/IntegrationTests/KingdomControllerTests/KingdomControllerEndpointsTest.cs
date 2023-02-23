using System.Net;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Kingdoms;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using TribesTests.Helpers;

namespace TribesTests.IntegrationTests.KingdomControllerTests;

public class KingdomControllerEndpointsTest : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<TestTokenHelper>
    {
        private readonly HttpClient _client;

        public KingdomControllerEndpointsTest(WebApplicationFactory<Program> factory, TestTokenHelper testTokenHelper)
        {
            _client = factory.CreateClient(); 
            string token = testTokenHelper.GenerateTestToken(1); 
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}"); 
        }

        [Theory]
        [MemberData(nameof(IndexOkTestData))]
        public async Task IndexOkTest(GetAllKingdomsResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.GetAsync("/api/kingdoms");
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetAllKingdomsResponse>();
            var actualStatusCode = actualResult.StatusCode;

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }
        
        public static IEnumerable<object[]> IndexOkTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new GetAllKingdomsResponse(new List<KingdomDTO> 
                    {
                        new KingdomDTO()
                        {
                            Id = 1,
                            Name = "Avalon - test",
                            Owner = "King Richard - test",
                            OwnerId = 1,
                            World = "Svetozor - test",
                            WorldId = 1
                        },

                        new KingdomDTO()
                        {
                            Id = 2,
                            Name = "Matrix - test",
                            Owner = "tkachlik",
                            OwnerId = 2,
                            World = "Lucerna - test",
                            WorldId = 2
                        }
                    }),
                    HttpStatusCode.OK 
                }
            };
        
        [Theory]
        [MemberData(nameof(GetMyKingdomsTestData))]
        public async Task GetMyKingdomTest(GetAllMyKingdomsResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.GetAsync("/api/kingdoms/get-my-kingdoms");
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetAllMyKingdomsResponse>();
            var actualStatusCode = actualResult.StatusCode;

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }
        
        public static IEnumerable<object[]> GetMyKingdomsTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new GetAllMyKingdomsResponse(new List<MyKingdomDTO> 
                    {
                        new MyKingdomDTO()
                        {
                            Id = 1,
                            Name = "Avalon - test",
                            World = "Svetozor - test",
                            WorldId = 1
                        },
                    }),
                    HttpStatusCode.OK 
                }
            };
        
        [Theory]
        [MemberData(nameof(ShowOkTestData))]
        public async Task ShowOkTest(string url, GetKingdomResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.GetAsync(url);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetKingdomResponse>();

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowOkTestData =>
            new List<object[]>
            {
                new object[]
                {
                    "/api/kingdoms/1", // URL with Player ID

                    new GetKingdomResponse(new KingdomStatsDTO()
                    {
                        Id = 1,
                        Name = "Avalon - test",
                        FoodAmount = 1000,
                        GoldAmount = 1000,
                        MaxStorage = 1200,
                        Owner = "King Richard - test",
                        OwnerId = 1,
                        TownhallLevel = 1,
                        World = "Svetozor - test",
                        WorldId = 1
                    }),

                    HttpStatusCode.OK // Status code returned
                }
            };
        
        [Theory]
        [MemberData(nameof(ShowErrorTestData))]
        public async Task ShowErrorTest(string url, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.GetAsync(url);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ShowErrorTestData =>
            new List<object[]>
            {
                new object[]
                {
                    "api/kingdoms/1000", //URL with incorrect format of Player ID

                    new ErrorResponse(404, "KingdomID", "Kingdom not found."),

                    HttpStatusCode.NotFound // Status code returned
                },

                new object[]
                {
                    "/api/kingdoms/-44",

                    new ErrorResponse(404, "KingdomID", "Kingdom not found."),

                    HttpStatusCode.NotFound
                }
            };
    }


    