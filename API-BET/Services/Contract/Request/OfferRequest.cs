using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace API_BET.Services.Contract
{
    public class OfferRequest
    {
        public long? OfferId { get; set; }
        
        public double? Odd { get; set; }
    }
}
