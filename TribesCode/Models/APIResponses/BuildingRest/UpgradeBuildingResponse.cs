using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses;

public class UpgradeBuildingResponse : IResponse
{
    public string Status { get; } = "Ok";
}