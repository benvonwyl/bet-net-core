using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API_BET.Services.Contract
{
    public class BetRequest
    {
        public List<OfferRequest> Offers { get; set; }
        
        public Guid? CustomerId { get; set; }

        public double? Stake { get; set; }
    }
}
