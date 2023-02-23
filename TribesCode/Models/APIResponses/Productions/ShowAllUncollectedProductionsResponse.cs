using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Productions
{
    public class ShowUncollectedProductionsResponse: IResponse
    {
        public string Status { get; } = "Ok";
        public List<ProductionDTO> UncollectedProductions { get; init; }

        public ShowUncollectedProductionsResponse() { }
        public ShowUncollectedProductionsResponse(List<ProductionDTO> uncollectedProductions)
        {
            UncollectedProductions = uncollectedProductions;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ShowUncollectedProductionsResponse;

            if (other == null) return false;

            if (Status != other.Status || 
                UncollectedProductions.Count != other.UncollectedProductions.Count) 
                return false;

            for (int i = 0; i < UncollectedProductions.Count; i++)
            {
                if (!UncollectedProductions[i].Equals(other.UncollectedProductions[i])) return false;
            }

            return true;
        }
    }
}