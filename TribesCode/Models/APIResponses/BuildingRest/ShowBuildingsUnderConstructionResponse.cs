using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses;

public class ShowBuildingsUnderConstructionResponse :IResponse
{
    public string Status { get; } = "Ok";
    public List<UnderConstructionDTO> UnderConstructionDtos { get; set; }
    
    public ShowBuildingsUnderConstructionResponse(List<UnderConstructionDTO> list) 
    {
            UnderConstructionDtos = list; 
    }
    
}