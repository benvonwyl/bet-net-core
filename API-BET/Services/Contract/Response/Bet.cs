using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace API_BET.Services.Contract
{
    public class Bet
    {
        public List<Offer> Offers { get; internal set; }

        public Guid CustomerId { get; internal set; }

        public double Stake { get; internal set; }

        public double TotalOdds { get; internal set; }
        public string Id { get; internal set; }
        public double PotentialWinning { get; internal set; }
    }
}
