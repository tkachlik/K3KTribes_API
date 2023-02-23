using dusicyon_midnight_tribes_backend.Models.APIResponses.Players;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TribesTests.Helpers;

namespace TribesTests.IntegrationTests.PlayerControllerTests
{
    public class PlayerControllerEndpointTests : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<TestTokenHelper>
    {
        private readonly HttpClient _client;

        public PlayerControllerEndpointTests(WebApplicationFactory<Program> factory, TestTokenHelper testTokenHelper)
        {
            _client = factory.CreateClient(); // generate a 15-minute JWT via helper method written for tests
            string token = testTokenHelper.GenerateTestToken(1); // imitate 1st player in the DB login
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}"); // add authorization JWT into header
        }

        [Theory]
        [MemberData(nameof(Index_OkTest_Data))]
        public async Task Index_OkTest(GetAllPlayersResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.GetAsync("/api/players");
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetAllPlayersResponse>();
            var actualStatusCode = actualResult.StatusCode;

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> Index_OkTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    new GetAllPlayersResponse(new List<PlayerDTO> // API RESPONSE EXPECTED
                    {
                        new PlayerDTO()
                        {
                            Id = 1,
                            UserName = "King Richard - test",
                            VerifiedAt = "2023.01.01. 00:00:00"
                        },

                        new PlayerDTO()
                        {
                            Id = 2,
                            UserName = "tkachlik",
                            VerifiedAt = null
                        }
                    }),

                    HttpStatusCode.OK // STATUS CODE RETURNED
                }
            };

        [Theory]
        [MemberData(nameof(Show_OkTest_Data))]
        public async Task Show_OkTest(string url, GetPlayerByIdResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.GetAsync(url);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetPlayerByIdResponse>();

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> Show_OkTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    "/api/players/1", // URL with Player ID

                    new GetPlayerByIdResponse(new PlayerDTO
                    {
                        Id = 1,
                        UserName = "King Richard - test",
                        VerifiedAt = "2023.01.01. 00:00:00"
                    }),

                    HttpStatusCode.OK // Status code returned
                },

                new object[]
                {
                    "/api/players/2",

                    new GetPlayerByIdResponse(new PlayerDTO
                    {
                        Id = 2,
                        UserName = "tkachlik",
                        VerifiedAt = null
                    }),

                    HttpStatusCode.OK
                },

                new object[]
                {
                    "/api/players/00001",

                    new GetPlayerByIdResponse(new PlayerDTO
                    {
                        Id = 1,
                        UserName = "King Richard - test",
                        VerifiedAt = "2023.01.01. 00:00:00"
                    }),

                    HttpStatusCode.OK
                }
            };

        [Theory]
        [MemberData(nameof(Show_ErrorTest_Data))]

        public async Task Show_ErrorTest(string url, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.GetAsync(url);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> Show_ErrorTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    "api/players/1000", //URL with incorrect format of Player ID

                    new ErrorResponse(404, "id", "Player not found."), // Error message returned

                    HttpStatusCode.NotFound // Status code returned
                },

                new object[]
                {
                    "/api/players/-13",

                    new ErrorResponse(400, "id", "Must be a positive integer."),

                    HttpStatusCode.BadRequest
                }
            };
    }
}