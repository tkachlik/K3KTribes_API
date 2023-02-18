using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses;

public class ConstructionOptionsResponse : IResponse
{
    public string Status { get; } = "Ok";
    public List<ConstructionOptionsDTO> Options { get; set; }

    public ConstructionOptionsResponse(List<ConstructionOptionsDTO> options)
    {
        Options = options;
    }
}