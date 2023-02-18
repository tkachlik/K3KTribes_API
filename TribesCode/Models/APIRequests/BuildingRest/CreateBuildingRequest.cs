using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.BuildingRest;

public class CreateBuildingRequest
{
    [Required(ErrorMessage = "Field is required."),
     StringLength(4, ErrorMessage = "Can only be 'mine' or 'farm'.")]
    public string BuildingType { get; set; }

    [Required(ErrorMessage = "Field is required."),
    Range(1,int.MaxValue)]
    public int KingdomId { get; set; }

}