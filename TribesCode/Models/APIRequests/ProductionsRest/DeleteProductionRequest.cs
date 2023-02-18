using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.ProductionsRest
{
    public class DeleteProductionRequest
    {
        [Required(ErrorMessage = "Field is required."),
            Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer.")]
        public int ProductionId { get; set; }
    }
}