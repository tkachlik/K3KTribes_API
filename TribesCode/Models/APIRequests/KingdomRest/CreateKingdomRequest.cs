using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.KingdomRest;

public class CreateKingdomRequest
{
    [Required(ErrorMessage = "Field is required."),
     MinLength(4, ErrorMessage = "Must be at least 4 characters long.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Field is required."),
    Range(0,19,ErrorMessage = "Number has to be between 0 and 19.")]
    public int Coordinate_x { get; set; }
    
    [Required(ErrorMessage = "Field is required."),
     Range(0,19,ErrorMessage = "Number has to be between 0 and 19.")]
    public int Coordinate_y { get; set; }
    
    [Required(ErrorMessage = "Field is required."),
        Range(1,int.MaxValue)]
    public int WorldId { get; set; }
}