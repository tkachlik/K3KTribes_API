using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Worlds;

public class GetAllWorldsResponse : IResponse
{
    public string Status { get; } = "Ok";
    public List<WorldDTO> WorldDTOs { get; init; }

    public override bool Equals(object? obj)
    {
        var other = obj as GetAllWorldsResponse;

        if (other == null) return false;

        if (Status != other.Status || WorldDTOs.Count != other.WorldDTOs.Count) return false;

        for (int i = 0; i < WorldDTOs.Count; i++)
        {
            if (!WorldDTOs[i].Equals(other.WorldDTOs[i])) return false;
        }

        return true;
    }
}
