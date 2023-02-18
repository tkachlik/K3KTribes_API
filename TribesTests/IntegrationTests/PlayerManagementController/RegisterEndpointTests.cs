using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TribesTests.Helpers;

namespace TribesTests.IntegrationTests.PlayerManagementController
{
    public class RegisterEndpointTests : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<TestTokenHelper>, IClassFixture<DirectSqlHelper>
    {
        private readonly HttpClient _client;
        private readonly DirectSqlHelper _directSqlHelper;

        public RegisterEndpointTests(WebApplicationFactory<Program> factory, TestTokenHelper testTokenHelper, DirectSqlHelper directSqlHelper)
        {
            _client = factory.CreateClient();
            string token = testTokenHelper.GenerateTestToken(1);
            _directSqlHelper = directSqlHelper;
        }

        // After running this test, you must manually delete two newly added Players from database - no time to automate this before sprint's end.

        [Theory]
        [MemberData(nameof(Register_OkTest_Data))]
        public async Task Register_OkTest(PlayerRegistrationRequest request, PlayerRegistrationResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.PostAsJsonAsync("/api/playermanagement/register/", request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<PlayerRegistrationResponse>();

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));

            // Cleanup
            _directSqlHelper.DeleteCreatedPlayer();
        }

        public static IEnumerable<object[]> Register_OkTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    new PlayerRegistrationRequest()
                    {
                        UserName = "Integration test user 1",
                        Email = "test1@integration.app",
                        Password = "heslo123",
                        PasswordAgain = "heslo123"
                    },

                    new PlayerRegistrationResponse
                    (
                        new PlayerCreatedDTO()
                        {
                            UserName = "Integration test user 1"
                        }
                    ),

                    HttpStatusCode.OK
                },

                new object[]
                {
                   new PlayerRegistrationRequest()
                    {
                        UserName = "Integration test user 2",
                        Email = "test2@integration.app",
                        Password = "heslo123",
                        PasswordAgain = "heslo123"
                    },

                    new PlayerRegistrationResponse
                    (
                        new PlayerCreatedDTO()
                        {
                            UserName = "Integration test user 2"
                        }
                    ),

                    HttpStatusCode.OK
                },
            };

        [Theory]
        [MemberData(nameof(Register_ValidationErrorTest_Data))]

        public async Task Register_ValidationErrorTest(PlayerRegistrationRequest request, ValidationResultModel expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.PostAsJsonAsync("/api/playermanagement/register", request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ValidationResultModel>();

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> Register_ValidationErrorTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    new PlayerRegistrationRequest()
                    {
                        UserName = "hrk",
                        Email = "brk",
                        Password = "heslo",
                        PasswordAgain = "heslo123"
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
                                Field = "Email",
                                ErrorMessage = "Invalid format."
                            },

                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Must be at least 8 characters long."
                            },

                            new ValidationError()
                            {
                                Field = "PasswordAgain",
                                ErrorMessage = "Passwords do not match."
                            }
                        }
                    },

                    HttpStatusCode.BadRequest
                },

                new object[]
                {
                    new PlayerRegistrationRequest()
                    {
                        UserName = "",
                        Email = "",
                        Password = "",
                        PasswordAgain = ""
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
                                Field = "UserName",
                                ErrorMessage = "Field is required."
                            },

                            new ValidationError()
                            {
                                Field = "Email",
                                ErrorMessage = "Invalid format."
                            },

                            new ValidationError()
                            {
                                Field = "Email",
                                ErrorMessage = "Field is required."
                            },

                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Must be at least 8 characters long."
                            },

                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Field is required."
                            },

                            new ValidationError()
                            {
                                Field = "PasswordAgain",
                                ErrorMessage = "Field is required."
                            }
                        }
                    },

                    HttpStatusCode.BadRequest
                },

                new object[]
                {
                    new PlayerRegistrationRequest() { },

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
                                Field = "Email",
                                ErrorMessage = "Field is required."
                            },

                            new ValidationError()
                            {
                                Field = "Password",
                                ErrorMessage = "Field is required."
                            },

                            new ValidationError()
                            {
                                Field = "PasswordAgain",
                                ErrorMessage = "Field is required."
                            }
                        }
                    },

                    HttpStatusCode.BadRequest
                }
            };

        [Theory]
        [MemberData(nameof(Register_DatabaseErrorTest_Data))]
        public async Task Register_DatabaseErrorTest(PlayerRegistrationRequest request, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            // Arrange & Act
            var actualResult = await _client.PostAsJsonAsync("/api/playermanagement/register", request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> Register_DatabaseErrorTest_Data =>
            new List<object[]>
            {
                new object []
                {
                    new PlayerRegistrationRequest()
                    {
                        UserName = "tkachlik",
                        Email = "tkachlik@gmail.com",
                        Password = "heslo123",
                        PasswordAgain = "heslo123"
                    },

                    new ErrorResponse(409, "UserName", "Exists already.", "Email", "Exists already."),

                    HttpStatusCode.Conflict
                },

                new object[]
                {
                    new PlayerRegistrationRequest()
                    {
                        UserName = "tkachlik",
                        Email = "tkachli@gmail.com",
                        Password = "heslo123",
                        PasswordAgain = "heslo123"
                    },

                    new ErrorResponse(409, "UserName", "Exists already."),

                    HttpStatusCode.Conflict
                },

                new object []
                {
                    new PlayerRegistrationRequest()
                    {
                        UserName = "tkachli",
                        Email = "tkachlik@gmail.com",
                        Password = "heslo123",
                        PasswordAgain = "heslo123"
                    },

                    new ErrorResponse(409, "Email", "Exists already."),

                    HttpStatusCode.Conflict
                }
            };
    }
}