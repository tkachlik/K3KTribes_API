using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement
{
    public class PlayerRegistrationRequest
    {
        [Required(ErrorMessage = "Field is required."),
            MinLength(4, ErrorMessage = "Must be at least 4 characters long.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Field is required."),
            EmailAddress(ErrorMessage = "Invalid format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field is required."),
            MinLength(8, ErrorMessage = "Must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field is required."),
            Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string PasswordAgain { get; set; }
    }
}