using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace API_BET.Model
{
    public class OfferModel
    {
        [Required]
        public IdentifierModel Identifier { get; set; }

        [Required]
        public int Status { get; set; }

        [Range(1, 20000)]
        [Required]
        public double Odds { get; set; }
    }
}
