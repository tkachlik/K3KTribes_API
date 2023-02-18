using dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TribesTests.Helpers;

namespace TribesTests.IntegrationTests.PlayerManagementController
{
    public class LoginPlayerEndpointTests : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<TestTokenHelper>
    {
        private readonly HttpClient _client;
        private readonly TestTokenHelper _testTokenService;
        private readonly string _uri;

        public LoginPlayerEndpointTests(WebApplicationFactory<Program> factory, TestTokenHelper testTokenService)
        {
            _client = factory.CreateClient();
            _testTokenService = testTokenService;
            _uri = "/api/playermanagement/login";
        }

        [Theory]
        [MemberData(nameof(LoginPlayer_OkTest_Data))]
        public async Task LoginPlayer_OkTest(PlayerLoginRequest request, HttpStatusCode expectedStatusCode)
        {
            var actualResult = await _client.PostAsJsonAsync(_uri, request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<PlayerLoginResponse>();
            
            var expectedResponse = new PlayerLoginResponse(string.Empty);

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
            Assert.True(_testTokenService.CheckTokenValidity(actualResponse.AuthenticationToken));
        }

        public static IEnumerable<object[]> LoginPlayer_OkTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    new PlayerLoginRequest()
                    {
                        UserName = "King Richard - test",
                        Password = "heslo123"
                    },

                    HttpStatusCode.OK
                },

                new object[]
                {
                    new PlayerLoginRequest()
                    {
                        UserName = "tkachlik",
                        Password = "heslo123"
                    },

                    HttpStatusCode.OK
                }
            };

        [Theory]
        [MemberData(nameof(LoginPlayer_ValidationErrorTest_Data))]
        public async Task LoginPlayer_ValidationErrorTest (PlayerLoginRequest request, ValidationResultModel expectedResponse)
        {
            var actualResult = await _client.PostAsJsonAsync(_uri, request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ValidationResultModel>();

            var expectedStatusCode = HttpStatusCode.BadRequest;

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> LoginPlayer_ValidationErrorTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    new PlayerLoginRequest()
                    {
                        UserName = "hrk",
                        Password = "frk"
                    },

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "UserName",
                                ErrorMessage = "Must be at least 4 characters long."
                            },

                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Must be at least 8 characters long."
                            }
                        }
                    }
                },

                new object[]
                {
                    new PlayerLoginRequest()
                    {
                        UserName = "hrk",
                        Password = "12345678"
                    },

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "UserName",
                                ErrorMessage = "Must be at least 4 characters long."
                            }
                        }
                    }
                },

                new object[]
                {
                    new PlayerLoginRequest()
                    {
                        UserName = "1234",
                        Password = "frk"
                    },

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Must be at least 8 characters long."
                            }
                        }
                    }
                },

                new object[]
                {
                    new PlayerLoginRequest()
                    {
                        UserName = "",
                        Password = ""
                    },

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "UserName",
                                ErrorMessage = "Must be at least 4 characters long."
                            },

                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Must be at least 8 characters long."
                            },

                            new ValidationError()
                            {
                                Field = "UserName",
                                ErrorMessage = "Field is required."
                            },

                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Field is required."
                            }
                        }
                    }
                },

                new object[]
                {
                    new PlayerLoginRequest() {},

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "UserName",
                                ErrorMessage = "Field is required."
                            },

                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Field is required."
                            }
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(PlayerLogin_DatabaseErrorTest_Data))]
        public async Task PlayerLogin_DatabaseErrorTest(PlayerLoginRequest request, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            var actualResult = await _client.PostAsJsonAsync(_uri, request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> PlayerLogin_DatabaseErrorTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    new PlayerLoginRequest()
                    {
                        UserName = "standapanda",
                        Password = "DFGHJKLKJHGF"
                    },

                    new ErrorResponse(404, "UserName", "Doesn't exist."),

                    HttpStatusCode.NotFound,
                },

                new object[]
                {
                    new PlayerLoginRequest()
                    {
                        UserName = "King Richard - test",
                        Password = "heslo1234"
                    },

                    new ErrorResponse(401, "Password", "Doesn't match the username."),

                    HttpStatusCode.Unauthorized
                }
            };
    }
}