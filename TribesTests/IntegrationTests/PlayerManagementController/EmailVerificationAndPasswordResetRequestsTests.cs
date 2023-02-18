using dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TribesTests.Helpers;

namespace TribesTests.IntegrationTests.PlayerManagementController
{
    public class EmailVerificationAndPasswordResetRequestsTests : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<TestTokenHelper>
    {
        private readonly HttpClient _client;
        private readonly TestTokenHelper _testTokenService;

        public EmailVerificationAndPasswordResetRequestsTests(WebApplicationFactory<Program> factory, TestTokenHelper testTokenService)
        {
            _client = factory.CreateClient();
            _testTokenService = testTokenService;
        }

        [Theory]
        [MemberData(nameof(EmailVerificationRequest_DatabaseErrorTest_Data))]
        public async Task EmailVerificationRequest_DatabaseErrorTest(ErrorResponse expectedResponse)
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_testTokenService.GenerateTestToken(1)}");

            var actualResult = await _client.PostAsJsonAsync("/api/playermanagement/verify-your-email", "");
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();
            
            var expectedStatusCode = HttpStatusCode.BadRequest;

            _client.DefaultRequestHeaders.Remove("Authorization");

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> EmailVerificationRequest_DatabaseErrorTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    new ErrorResponse(400, "VerificationToken", "Already sent. --- If you need the token resend, please use the 'resend verification token' endpoint.")
                }
            };

        [Theory]
        [MemberData(nameof(ResendEmailVerificationToken_DatabaseErrorTest_Data))]
        public async Task ResendEmailVerificationToken_DatabaseErrorTest(int playerId, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_testTokenService.GenerateTestToken(playerId)}");

            var actualResult = await _client.PostAsJsonAsync("api/email/verify/resend", "");
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            _client.DefaultRequestHeaders.Remove("Authorization");

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> ResendEmailVerificationToken_DatabaseErrorTest_Data =>
            new List<object[]>()
            {
                new object[]
                {
                    1,

                    new ErrorResponse(400, "Email", "Verified already."),

                    HttpStatusCode.BadRequest
                },

                new object[]
                {
                    2,

                    new ErrorResponse(404, "VerificationToken", "Not send yet. --- Please use the 'verify-your-email' endpoint."),

                    HttpStatusCode.NotFound
                }
            };

        [Theory]
        [MemberData(nameof(PasswordResetRequest_ValidationErrorTest_Data))]
        public async Task SendPasswordResetToken_ValidationErrorTest(SendPasswordResetTokenRequest request, ValidationResultModel expectedResponse)
        {
            var actualResult = await _client.PostAsJsonAsync("/api/playermanagement/reset-password-request", request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ValidationResultModel>();

            var expectedStatusCode = HttpStatusCode.BadRequest;

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> PasswordResetRequest_ValidationErrorTest_Data =>
            new List<object[]>()
            {
                new object[]
                {
                    new SendPasswordResetTokenRequest()
                    {
                        Email = "",
                    },

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "Email",
                                ErrorMessage = "Field is required."
                            },

                            new ValidationError()
                            {
                                Field = "Email",
                                ErrorMessage = "Must be in email address format."
                            }
                        }
                    }
                },

                new object[]
                {
                    new SendPasswordResetTokenRequest()
                    {
                        Email = "frbr.cr"
                    },

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "Email",
                                ErrorMessage = "Must be in email address format."
                            }
                        }
                    }
                },

                new object[]
                {
                    new SendPasswordResetTokenRequest() { },

                    new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "Email",
                                ErrorMessage = "Field is required."
                            }
                        }
                    }
                },
            };

        [Theory]
        [MemberData(nameof(SendPasswordResetToken_DatabaseErrorTest_Data))]
        public async Task SendPasswordResetToken_DatabaseErrorTest(SendPasswordResetTokenRequest request, ErrorResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            var actualResult = await _client.PostAsJsonAsync("/api/playermanagement/reset-password-request", request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> SendPasswordResetToken_DatabaseErrorTest_Data =>
            new List<object[]>
            {
                new object[]
                {
                    new SendPasswordResetTokenRequest()
                    {
                        Email = "pimpili@dimpili.schudlibuhm"
                    },

                    new ErrorResponse(404, "Email", "No such email found. --- Only registered and verified emails can be used for password reset"),

                    HttpStatusCode.NotFound
                },

                new object[]
                {
                    new SendPasswordResetTokenRequest()
                    {
                        Email = "tkachlik@gmail.com"
                    },

                    new ErrorResponse(400, "Email", "The entered email address has not been verified yet! --- Only verified emails can be used for password reset."),

                    HttpStatusCode.BadRequest
                }
            };
    }
}