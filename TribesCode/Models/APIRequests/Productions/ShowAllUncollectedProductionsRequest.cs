﻿using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.Productions
{
    public class ShowAllUncollectedProductionsRequest
    {
        [Required(ErrorMessage = "Field is required."),
            Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer.")]
        public int KingdomId { get; set; }
    }
}