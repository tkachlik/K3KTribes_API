using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.WorldRest
{
    public class CreateWorldRequest
    {
        [Required(ErrorMessage = "Field is required."),
            MinLength(4, ErrorMessage = "Must be at least 4 characters long."),
            MaxLength(20, ErrorMessage = "Must be at max 20 characters long.")]
        public string Name { get; set; }
    }
}
