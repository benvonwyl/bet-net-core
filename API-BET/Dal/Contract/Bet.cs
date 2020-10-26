using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace API_BET.Dal.Contract
{
    public class Bet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get;  set; }

        public List<Offer> Offers { get;  set; }

        public Guid CustomerId { get;  set; }

        public double Stake { get;  set; }

        public double TotalOdds { get;  set; }

        public double PotentialWinning { get;  set; }
    }
}
