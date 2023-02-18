using CsvHelper;
using dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerRest;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TribesTests.Helpers;

namespace TribesTests.IntegrationTests.PlayerManagementController
{
    public class GetPlayerEndpointTests : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<TestTokenHelper>
    {
        private readonly HttpClient _client;
        private readonly TestTokenHelper _testTokenHelper;

        public GetPlayerEndpointTests(WebApplicationFactory<Program> factory, TestTokenHelper testTokenHelper)
        {
            _client = factory.CreateClient();
            _testTokenHelper = testTokenHelper;
        }

        [Theory]
        [MemberData(nameof(GetPlayer_OkTest_Data))]
        public async Task GetPlayer_OkTest(int playerId, GetPlayerByIdResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            string token = _testTokenHelper.GenerateTestToken(playerId);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var actualResult = await _client.PostAsync("api/playermanagement/get-player", null);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetPlayerByIdResponse>();

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> GetPlayer_OkTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    1,

                    new GetPlayerByIdResponse(new PlayerDTO()
                    {
                        Id = 1,
                        UserName = "King Richard - test",
                        VerifiedAt = "2023.01.01. 00:00:00"
                    }),

                    HttpStatusCode.OK
                },

                new object[]
                {
                    2,

                    new GetPlayerByIdResponse(new PlayerDTO()
                    {
                        Id = 2,
                        UserName = "tkachlik",
                        VerifiedAt = null
                    }),

                    HttpStatusCode.OK
                }
            };
    }
}