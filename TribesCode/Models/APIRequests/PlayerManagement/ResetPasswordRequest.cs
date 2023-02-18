using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Field is required.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Field is required."),
            MinLength(8, ErrorMessage = "Must be at least 8 characters long.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Field is required."),
            Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string NewPasswordAgain { get; set; }
    }
}