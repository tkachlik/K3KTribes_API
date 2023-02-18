using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.BuildingRest;

public class ShowBuildingsUnderConstructionRequest
{
    [Required(ErrorMessage = "Field is required."),
     Range(1,int.MaxValue)]
    public int KingdomId { get; set; }
}