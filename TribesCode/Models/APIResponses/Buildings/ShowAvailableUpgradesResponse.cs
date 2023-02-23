using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Buildings;

public class ShowAvailableUpgradesResponse : IResponse
{
    public string Status { get; } = "Ok";
    public List<UpgradeOptionsDTO> upgradeOptionDTOs { get; set; }

    public ShowAvailableUpgradesResponse(List<UpgradeOptionsDTO> list)
    {
        upgradeOptionDTOs = list;
    }
}