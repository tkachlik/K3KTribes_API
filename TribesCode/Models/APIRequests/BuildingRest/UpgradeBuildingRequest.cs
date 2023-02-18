using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.BuildingRest;

public class UpgradeBuildingRequest
{
    [Required(ErrorMessage = "Field is required."),
     Range(1,int.MaxValue)]
    public int BuildingId { get; set; }
}