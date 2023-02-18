using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.ProductionsRest
{
    public class ShowAvailableProductionOptionsRequest
    {
        [Required(ErrorMessage = "Field is required."),
            Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer.")]
        public int KingdomId { get; set; }
    }
}