using Moq;
using AutoMapper;
using dusicyon_midnight_tribes_backend.Services;
using dusicyon_midnight_tribes_backend.Services.Repositories;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Worlds;
using dusicyon_midnight_tribes_backend.Models.APIRequests.WorldRest;

namespace TribesTests.MockTest;

public class WorldServiceMockTest
{
    private readonly WorldService _sut; // system under test
    private readonly Mock <IGenericRepository> _genericRepoMock = new Mock<IGenericRepository> ();
    private readonly Mock <IWorldRepository> _worldRepoMock = new Mock<IWorldRepository> ();
    private readonly Mock <IPlayerRepository> _playerRepoMock = new Mock<IPlayerRepository> ();
    private readonly Mock <IMapper> _mapperMock = new Mock<IMapper> ();
    
    public WorldServiceMockTest()
    {
        _sut = new WorldService(_genericRepoMock.Object, 
                                _worldRepoMock.Object, 
                                _mapperMock.Object, 
                                _playerRepoMock.Object
                                );
    }

    [Fact]
    public void GetWorldById_ShouldReturnWorld_WhenWorldExist()
    {
        //Arrange
        var worldId = 1;
        var worldName = "TestName";
        var world = new World()
        {
            Id = worldId,
            Name = worldName,
            Kingdoms = new List<Kingdom>(),
            PlayerWorlds = new List<PlayerWorld>()
        };
        var worldDTO = new WorldDTO()
        {
            Id = world.Id,
            Name = world.Name,
            KingdomCount = world.Kingdoms.Count,
            PlayerNames = new List<string>() { "Standa"}
        };

        _worldRepoMock.Setup(x => x.GetWorldById(worldId)).Returns(world);
        _worldRepoMock.Setup(x => x.GetAllPlayerNamesInTheWorldById(worldId)).Returns(worldDTO.PlayerNames);
        _mapperMock.Setup(x => x.Map<WorldDTO>(world)).Returns(worldDTO);

        //Act
        IResponse worldSUT = _sut.GetWorldById(worldId);
        var response = (GetWorldByIdResponse)worldSUT;

        //Asssert
        Assert.Equal("Ok", response.Status);
        Assert.Equal(worldId, response.World.Id);
        Assert.Equal(worldName, response.World.Name);
        Assert.True(response.World.KingdomCount == 0);
    }

    [Fact]
    public void GetWorldById_ShouldReturnErrorResponse_WhenWorldDoesNotExist()
    {
        //Arrange
        _worldRepoMock.Setup(x => x.GetWorldById(It.IsAny<int>())).Returns(() => null);

        //Act
        IResponse worldSUT = _sut.GetWorldById(9999);
        var response = (ErrorResponse)worldSUT;

        //Asssert
        Assert.Equal(404, response.StatusCode);
        Assert.Equal("This world no exist", response.Errors[0].ErrorMessage);
        Assert.Empty(response.Errors[0].Field);
    }

    [Fact]
    public void GetWorldById_ShouldReturnErrorResponse_WhenIdIsLessOrEqualZero()
    {
        //Arrange
        _worldRepoMock.Setup(x => x.GetWorldById(It.IsAny<int>())).Returns(() => null);

        //Act
        IResponse worldSUT = _sut.GetWorldById(-13);
        var response = (ErrorResponse)worldSUT;

        //Asssert
        Assert.Equal(400, response.StatusCode);
        Assert.Equal("Id must be a positive integer and not zero.", response.Errors[0].ErrorMessage);
        Assert.Empty(response.Errors[0].Field);
    }

    [Fact]
    public void GetAllWorlds_ShouldReturnListOfWorld_WhenIsAny()
    {
        //Arrange
        var worlds = Worlds;
        var worldsDTO = WorldsDTO;
        _worldRepoMock.Setup(x => x.GetAllWorlds()).Returns(worlds);
        for(int i =0; i<worlds.Count;i++)
        {
            _mapperMock.Setup(x => x.Map<WorldDTO>(worlds[i])).Returns(worldsDTO[i]);
        }

        //Act
        IResponse worldSUT = _sut.GetAllWorlds();
        var response = (GetAllWorldsResponse)worldSUT;

        //Assert
        Assert.Equal("Ok", response.Status);
        Assert.IsType<List<WorldDTO>>(response.WorldDTOs);
        Assert.Equal(worlds.Count, response.WorldDTOs.Count);
    }

    [Fact]
    public void GetAllWorlds_ShouldReturnErrorResponse_WhenIsNull()
    {
        //Arrange
        var worlds = Worlds;
        var worldsDTO = WorldsDTO;
        _worldRepoMock.Setup(x => x.GetAllWorlds()).Returns(() => null);
     
        //Act
        IResponse worldSUT = _sut.GetAllWorlds();
        var response = (ErrorResponse)worldSUT;
        //Assert
        Assert.Equal(500, response.StatusCode);
        Assert.Equal("Unknown internal server error.", response.Errors[0].ErrorMessage);
        Assert.Empty(response.Errors[0].Field);
    }

    [Fact]
    public void CreateWorld_CheckWorldNameExist_ShouldReturnProperErrorResponse()
    {
        //Arrange
        var playerId = 5;
        var createWorldRequest = new CreateWorldRequest() { Name = "TestName"};
        _worldRepoMock.Setup(x => x.CheckWorldNameExist(createWorldRequest.Name)).Returns(true);

        //Act
        IResponse worldSUT = _sut.CreateWorld(createWorldRequest, playerId);
        var response = (ErrorResponse)worldSUT;

        //Assert
        Assert.Equal(400, response.StatusCode);
        Assert.Equal("This world Name already exist", response.Errors[0].ErrorMessage);
        Assert.Empty(response.Errors[0].Field);
    }

    [Fact] //TODO to make this test working, must learn how to mock constructors
                // world from tests not same as world created in service method CreateWorld() => 
                // => NUll reference exception => object is refer by the place in memory
    public void CreateWorld_ShouldReturnWorldDTOWithPlayer_WhenAllInputsAreValid()
    {
        //Arrange
        var playerId = 5;
        var createWorldRequest = new CreateWorldRequest() { Name = "MessKingdom" };
        var player = new Player(playerId,"Alenka", "ala@moje.cz", "asdklfjbvh4535asdfsvg");
      
        var worldId = 5;
        var worldName = createWorldRequest.Name;
        var world = new World()
        {
            Id = worldId,
            Name = worldName,
            Kingdoms = new List<Kingdom>(),
            PlayerWorlds = new List<PlayerWorld>()
        };
        var worldDTO = new WorldDTO()
        {
            Id = world.Id,
            Name = world.Name,
            KingdomCount = world.Kingdoms.Count,
            PlayerNames = new List<string>() { player.UserName }
        };
       

        _worldRepoMock.Setup(x => x.CheckWorldNameExist(createWorldRequest.Name)).Returns(false);
        _playerRepoMock.Setup(x => x.GetPlayerById(playerId)).Returns(player);
        _genericRepoMock.Setup(x => x.Save()).Returns(true);
        _worldRepoMock.Setup(x => x.CreateWorld(world));
        //var worldMock = new Mock<World>(createWorldRequest.Name).Object;
        _worldRepoMock.Setup(x => x.GetAllPlayerNamesInTheWorldById(worldDTO.Id)).Returns(worldDTO.PlayerNames);
        _mapperMock.Setup(x => x.Map<WorldDTO>(It.IsAny<World>())).Returns(worldDTO);
        
        //Act
        IResponse worldSUT = _sut.CreateWorld(createWorldRequest, playerId);
        var response = (CreateWorldResponse)worldSUT;

        //Assert
        Assert.Equal("Ok", response.Status);
        Assert.Equal("Alenka", response.World.PlayerNames[0]);
    }

    [Fact]
    public void CreateWorld_SaveChangesErrorResponse_WhenRepoSaveReturnZero()
    {
        //Arrange
        var playerId = 5;
        var createWorldRequest = new CreateWorldRequest() { Name = "MessKingdom" };
        var world = new World(createWorldRequest.Name);
        var player = new Player(playerId, "Alenka", "ala@moje.cz", "asdklfjbvh4535asdfsvg");

        _worldRepoMock.Setup(x => x.CheckWorldNameExist(createWorldRequest.Name)).Returns(false);
        _playerRepoMock.Setup(x => x.GetPlayerById(playerId)).Returns(player);
        _genericRepoMock.Setup(x => x.Save()).Returns(false);

        //Act
        IResponse worldSUT = _sut.CreateWorld(createWorldRequest, playerId);
        var response = (SaveChangesErrorResponse)worldSUT;

        //Assert
        Assert.Equal(400, response.StatusCode);
        Assert.Equal("Saving to database failed. Unknown error.", response.Errors[0].ErrorMessage);
        Assert.Empty(response.Errors[0].Field);
    }

    private List<World> Worlds =>
    new List<World>
    {
        new World ("Asgard"),
        new World ("PepaWorld"),
        new World ("Abungo"),
    };

    private List<WorldDTO> WorldsDTO =>
        new List<WorldDTO>
        {
            new WorldDTO{ Name = Worlds[0].Name },
            new WorldDTO{ Name = Worlds[1].Name },
            new WorldDTO{ Name = Worlds[2].Name },
        };
    //Arrange

    //Act

    //Assert

}
