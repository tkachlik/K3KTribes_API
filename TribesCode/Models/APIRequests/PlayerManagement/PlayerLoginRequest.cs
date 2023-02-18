using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement
{
    public class PlayerLoginRequest
    {
        [Required(ErrorMessage = "Field is required."),
            MinLength(4, ErrorMessage = "Must be at least 4 characters long.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Field is required."),
            MinLength(8, ErrorMessage = "Must be at least 8 characters long.")]
        public string Password { get; set; }
    }
}