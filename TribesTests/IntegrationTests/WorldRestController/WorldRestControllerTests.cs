using dusicyon_midnight_tribes_backend.Models.APIRequests.WorldRest;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Worlds;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TribesTests.Helpers;

namespace TribesTests.IntegrationTests.WorldRestController
{
    public class WorldRestControllerTests : IClassFixture<WebApplicationFactory<Program>> , IClassFixture<TestTokenHelper>
    {
        private readonly HttpClient _client;

        public WorldRestControllerTests(WebApplicationFactory<Program> factory, TestTokenHelper testTokenGenerator)
        {
            _client = factory.CreateClient();
            string token = testTokenGenerator.GenerateTestToken(1);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        [Theory]
        [MemberData(nameof(Index_Return_200_MemberData))]
        public async Task Index_ResponseIsEqualExpected(GetAllWorldsResponse expectedResponse) 
        {   // test automapper works propper way

            //Arrange
            var actualResult = await _client.GetAsync("/api/worlds");
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetAllWorldsResponse>();
            
            //Assert
            Assert.True(expectedResponse.Equals(actualResponse));
        }
        [Theory]
        [MemberData(nameof(Index_Return_200_MemberData))]
        public async Task Index_Return_200_WhenInputIsValid(GetAllWorldsResponse expectedResponse)
        {
            //Act
            var actualResult = await _client.GetAsync("/api/worlds");
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetAllWorldsResponse>();
            var actualStatusCode = actualResponse.Status.ToLower();

            //Assert
            var expectedStatusCode = expectedResponse.Status.ToLower();

            //Act
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        //[Theory]
        //[MemberData(nameof(Index_Return_500_MemberData))]
        public async Task Index_Return_500_WhenResultSourceIsNull(ErrorResponse expectedResponse)
        {
            //Act
            var actualResult = await _client.GetAsync("/api/worlds");
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            //Assert
            Assert.Equal(500, actualResponse.StatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        [Theory]
        [MemberData(nameof(Show_ReturnOk_MemberData))]
        public async Task Show_ReturnOk_WhenWorldWithInputIdExist(string url, GetWorldByIdResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var actualResult = await _client.GetAsync(url);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<GetWorldByIdResponse>();

            //Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        [Theory]
        [MemberData(nameof(Show_ReturnCorrectError_MemberData))]
        public async Task Show_ReturnCorrectError_WhenInputIsInvalid(string url, ErrorResponse expectedResonse, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var actualResult = await _client.GetAsync(url);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ErrorResponse>();

            //Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(expectedResonse.Equals(actualResponse));
        }

        //[Theory]
        //[MemberData(nameof(Create_ReturnOkAndAddWorld_MemberData))]
        public async Task Create_ReturnOkAndAddWorld_WhenInputIsValid(CreateWorldRequest request, CreateWorldResponse expectedResponse, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var actualResult = await _client.PostAsJsonAsync("/api/worlds/create", request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<CreateWorldResponse>();

            //Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            //Assert.True(expectedResponse.Equals(actualResponse));
        }

        [Theory]
        [MemberData(nameof(Create_ReturnError_MemberData))]
        public async Task Create_ReturnError_WhenInputIsInvalid(CreateWorldRequest request, ValidationResultModel validationResultModel, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var actualResult = await _client.PostAsJsonAsync("/api/worlds/create", request);
            var actualStatusCode = actualResult.StatusCode;
            var actualResponse = await actualResult.Content.ReadFromJsonAsync<ValidationResultModel>();


            //Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        public static IEnumerable<object[]> Create_ReturnError_MemberData =>
            new List<object[]>
            {
                new object[]
                {
                    new CreateWorldRequest()
                    {
                        Name = "ss"
                    },
                     new ValidationResultModel()
                    {
                        Errors = new List<ValidationError>()
                        {
                            new ValidationError()
                            {
                                Field = "",
                                ErrorMessage = "Must be at least 4 characters long."
                            },

                            //new ValidationError()
                            //{
                            //    Field = "",
                            //    ErrorMessage = "Invalid format."
                            //},

                            //new ValidationError()
                            //{
                            //    Field = "",
                            //    ErrorMessage = "Must be at least 8 characters long."
                            //},
                        }
                    },

                    HttpStatusCode.BadRequest
                },


            };

        public static IEnumerable<object[]> Create_ReturnOkAndAddWorld_MemberData =>
            new List<object[]>
            {
                new object[]
                {
                    new CreateWorldRequest()
                    {
                        Name = $"world"
                    },
                    new CreateWorldResponse()
                    {
                        World = new WorldDTO()
                        {
                            Name = $"world",
                            KingdomCount = 0,
                            PlayerNames = new List<string>(){ "King Richard - test" }
                        }
                    },

                    HttpStatusCode.OK // Status code returned
                },

                
            };

        public static IEnumerable<object[]> Show_ReturnOk_MemberData =>
            new List<object[]>
            {
                new object[]
                {
                    "/api/worlds/1", 

                    new GetWorldByIdResponse()
                    {
                        World = new WorldDTO()
                        {
                            Id = 1,
                            Name = "Svetozor - test",
                            KingdomCount = 1,
                            PlayerNames = new List<string>(){ "King Richard - test" }
                        }
                    },

                    HttpStatusCode.OK // Status code returned
                },

                new object[]
                {
                    "/api/worlds/2",

                    new GetWorldByIdResponse()
                    {
                        World = new WorldDTO()
                        {
                            Id = 2,
                            Name = "Lucerna - test",
                            KingdomCount = 1,
                            PlayerNames = new List<string>(){"tkachlik"}
                        }
                    },

                    HttpStatusCode.OK
                },

                new object[]
                {
                    "/api/worlds/00000000000000000002",

                    new GetWorldByIdResponse()
                    {
                        World = new WorldDTO()
                        {
                            Id = 2,
                            Name = "Lucerna - test",
                            KingdomCount = 1,
                            PlayerNames = new List<string>(){"tkachlik"}
                        }
                    },

                    HttpStatusCode.OK
                }
            };

        public static IEnumerable<object[]> Show_ReturnCorrectError_MemberData =>
            new List<object[]>
            {
                new object[]
                {
                    "api/worlds/9999", 

                    new ErrorResponse(404, "", "This world no exist"), // Error message returned

                    HttpStatusCode.NotFound // Status code returned
                },

                new object[]
                {
                    "api/worlds/-45",

                    new ErrorResponse(400, "", "Id must be a positive integer and not zero."),

                    HttpStatusCode.BadRequest
                },

                new object[]
                {
                    "api/worlds/0",

                    new ErrorResponse(400, "", "Id must be a positive integer and not zero."),

                    HttpStatusCode.BadRequest
                },
            };

        public static IEnumerable<object[]> Index_Return_500_MemberData =>
            new List<object[]>
            {
                new object[]
                {
                    new ErrorResponse()
                    {
                        StatusCode = 500,
                        Errors = new List<Error>()
                        {
                            new Error("", "Unknown internal server error.")
                        }
                    }
                }
            };

        public static IEnumerable<object[]> Index_Return_200_MemberData =>
            new List<object[]>
            {
                new object[]
                {
                    new GetAllWorldsResponse()
                    {
                        WorldDTOs = new List<WorldDTO>()
                        {
                            new WorldDTO() {
                                Id = 1,
                                Name = "Svetozor - test",
                                KingdomCount = 1,
                                PlayerNames = new List<string>(){ "King Richard - test" }
                            },
                            new WorldDTO() {
                                Id = 2,
                                Name = "Lucerna - test",
                                KingdomCount = 1,
                                PlayerNames = new List<string>(){"tkachlik"}
                            },
                        },
                    }
                }
            };
    }
}
