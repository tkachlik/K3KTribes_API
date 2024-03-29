﻿using System.ComponentModel.DataAnnotations;

namespace dusicyon_midnight_tribes_backend.Models.APIRequests.Productions
{
    public class ProduceResourceRequest
    {
        [Required(ErrorMessage = "Field is required."),
            Range(1,int.MaxValue, ErrorMessage = "Must be a positive integer.")]
        public int BuildingId { get; set; }

        [Required(ErrorMessage = "Field is required."),
            Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer.")]
        public int ProductionOptionId { get; set; }
    }
}