using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses;

public class CreateBuildingResponse : IResponse

{
    public string Status { get; } = "Ok";
    public BuildingCreatedDTO BuildingCreated { get; init; }

    public CreateBuildingResponse() { }

    public CreateBuildingResponse(BuildingCreatedDTO buildingCreated)
    {
        BuildingCreated = buildingCreated;
    }
}