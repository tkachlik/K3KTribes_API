using AutoMapper;
using dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.PlayerManagement;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using dusicyon_midnight_tribes_backend.Services;
using dusicyon_midnight_tribes_backend.Services.Repositories;
using Moq;

namespace TribesTests.UnitTests.PlayerManagementServiceTests
{
    public class RegisterTests
    {
        private readonly PlayerManagementService _sut;
        private readonly Mock<IGenericRepository> _repoMock;
        private readonly Mock<IPlayerRepository> _playerRepoMock;
        private readonly Mock<IEmailVerificationsRepository> _emailVerificationRepoMock;
        private readonly Mock<IForgottenPasswordsRepository> _forgottenPasswordRepoMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly PlayerRegistrationRequest _request;
        private readonly string _name;
        private readonly string _email;

        public RegisterTests()
        {
            _repoMock = new Mock<IGenericRepository>();
            _playerRepoMock = new Mock<IPlayerRepository>();
            _emailVerificationRepoMock = new Mock<IEmailVerificationsRepository>();
            _forgottenPasswordRepoMock = new Mock<IForgottenPasswordsRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _emailServiceMock = new Mock<IEmailService>();
            _mapperMock = new Mock<IMapper>();

            _sut = new PlayerManagementService
                (
                    _repoMock.Object,
                    _playerRepoMock.Object,
                    _emailVerificationRepoMock.Object,
                    _forgottenPasswordRepoMock.Object,
                    _tokenServiceMock.Object,
                    _emailServiceMock.Object,
                    _mapperMock.Object
                );

            _name = "name";
            _email = "email";

            _request = new PlayerRegistrationRequest()
            {
                UserName = _name,
                Email = _email,
                Password = "password",
                PasswordAgain = "password"
            };
        }

        [Fact]
        public void Register_ShouldReturnOk_WhenEverythingOk()
        {
            var playerProfileCreated = new PlayerCreatedDTO()
            {
                UserName = _name
            };

            SetUpPlayerRepoMockForUserNameAndEmailCheckTest("neither");
            _repoMock.Setup(x => x.Save()).Returns(true);
            _mapperMock.Setup(x => x.Map<PlayerCreatedDTO>(It.IsAny<Player>())).Returns(playerProfileCreated);

            var expectedResponse = new PlayerRegistrationResponse(playerProfileCreated);

            // Act
            var actualResponse = _sut.Register(_request);

            // Assert
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        [Fact]
        public void Register_ShouldReturnSaveChangesErrorResponses_IfSavingToDatabaseFailed()
        {
            // Arrange
            SetUpPlayerRepoMockForUserNameAndEmailCheckTest("neither");
            _repoMock.Setup(x => x.Save()).Returns(false);

            var expectedResponse = new SaveChangesErrorResponse();

            // Act
            var actualResponse = _sut.Register(_request);

            // Assert
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        [Theory]
        [MemberData(nameof(Register_ShouldReturnErrorResponse_WhenEitherUseerNameAndOrEmail_TestData))]
        public void Register_ShouldReturnErrorResponse_WhenEitherUseNameAndOrEmailAlreadyExist(string option, ErrorResponse expectedResponse)
        {
            // Arrange
            SetUpPlayerRepoMockForUserNameAndEmailCheckTest(option);

            // Act
            var actualResponse = _sut.Register(_request);

            // Assert
            Assert.True(expectedResponse.Equals(actualResponse));
        }

        public static IEnumerable<object[]> Register_ShouldReturnErrorResponse_WhenEitherUseerNameAndOrEmail_TestData => new List<object[]>
        {
            new object[]
            {
                "both",

                new ErrorResponse(409, "UserName", "Exists already.", "Email", "Exists already.")
            },

            new object[]
            {
                "name",

                new ErrorResponse(409, "UserName", "Exists already.")
            },

            new object[]
            {
                "email",

                new ErrorResponse(409, "Email", "Exists already.")
            }
        };

        // Helper functions
        private void SetUpPlayerRepoMockForUserNameAndEmailCheckTest(string option)
        {
            switch (option)
            {
                case "both":
                    _playerRepoMock.Setup(x => x.CheckPlayerExistsByName(_name)).Returns(true);
                    _playerRepoMock.Setup(x => x.CheckEmailExists(_email)).Returns(true);
                    break;

                case "name":
                    _playerRepoMock.Setup(x => x.CheckPlayerExistsByName(_name)).Returns(true);
                    _playerRepoMock.Setup(x => x.CheckEmailExists(_email)).Returns(false);
                    break;

                case "email":
                    _playerRepoMock.Setup(x => x.CheckPlayerExistsByName(_name)).Returns(false);
                    _playerRepoMock.Setup(x => x.CheckEmailExists(_email)).Returns(true);
                    break;

                case "neither":
                    _playerRepoMock.Setup(x => x.CheckPlayerExistsByName(_name)).Returns(false);
                    _playerRepoMock.Setup(x => x.CheckEmailExists(_email)).Returns(false);
                    break;

                default: break;
            }
        }
    }
}