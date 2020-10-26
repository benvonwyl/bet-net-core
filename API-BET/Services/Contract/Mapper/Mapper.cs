using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace API_BET.Services.Contract.Mapper
{
    public static class Mapper
    {
        public static API_BET.Dal.Contract.Offer ToOfferDB(this Offer o)
        {
            if (o == null)
            {
                return null;
            }

            return new API_BET.Dal.Contract.Offer() { Odd = o.Odd, OfferId = o.OfferId };
        }

        public static API_BET.Dal.Contract.Bet ToBetDB(this Bet b)
        {

            if (b == null) 
            {
                return null; 
            }

            return new API_BET.Dal.Contract.Bet
            {
                CustomerId = b.CustomerId,
                Offers = b.Offers.Select(t => t.ToOfferDB())?.ToList(),
                PotentialWinning = b.PotentialWinning,
                Stake = b.Stake,
                TotalOdds = b.TotalOdds
            };
        }


        public static Offer ToOfferService(this API_BET.Dal.Contract.Offer o)
        {
            if (o == null)
            {
                return null;
            }

            return new Offer() { Odd = o.Odd, OfferId = o.OfferId };
        }

        public static Bet ToBetService(this API_BET.Dal.Contract.Bet b)
        {

            if (b == null)
            {
                return null;
            }

            return new Bet
            {
                CustomerId = b.CustomerId,
                Offers = b.Offers.Select(t => t.ToOfferService())?.ToList(),
                PotentialWinning = b.PotentialWinning,
                Stake = b.Stake,
                TotalOdds = b.TotalOdds,
                Id = b.Id
            };
        }

    }
}
