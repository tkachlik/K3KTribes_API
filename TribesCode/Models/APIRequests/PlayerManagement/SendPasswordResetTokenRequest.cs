using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.PlayerManagement
{
    public class SendPasswordResetTokenRequest
    {
        [Required(ErrorMessage = "Field is required."),
            EmailAddress(ErrorMessage = "Must be in email address format.")]
        public string Email { get; set; }
    }
}