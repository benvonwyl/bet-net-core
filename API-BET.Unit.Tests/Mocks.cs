using API_BET.Model;
using API_BET.Services.Contract;
using System;
using System.Collections.Generic;

namespace API_BET_TESTS
{
    public static class Mocks
    {

        public static string DummyOfferApiResponse()
        {
            return "[{\"identifier\":{\"id\":502570803,\"isLive\":false},\"odds\":1.22,\"status\":1,\"iafc\":true,\"isBoostedOdd\":false}]";
        }

        public static List<OfferModel> DummyOfferClientResponse()
        {
            return new List<OfferModel>()
            {
                new OfferModel {
                    Identifier = new IdentifierModel { Id = 502570803 },
                    Odds = 1.22,
                    Status = 1
                },
                new OfferModel
                {
                    Identifier = new IdentifierModel { Id = 502570804 },
                    Odds = 1.10,
                    Status = 1
                }
            };
        }

        public static BetRequest DummyBetRequest()
        {
            return new BetRequest()
            {
                CustomerId = new Guid(),
                Stake = 100,
                Offers = new List<OfferRequest>()
                {
                    new OfferRequest() { Odd = 1.22, OfferId = 502570803 },
                    new OfferRequest() { Odd = 1.10, OfferId = 502570804 }
                }
            };
        }

        internal static API_BET.Dal.Contract.Bet BetDummyBetDatabaseResponse()
        {
            return new API_BET.Dal.Contract.Bet()
            {
                Id = "5f95f82f3303de0d5ed224fc",
                PotentialWinning = 134.2,
                CustomerId = new Guid(),
                Stake = 100,
                Offers = new List<API_BET.Dal.Contract.Offer>()
                {
                    new API_BET.Dal.Contract.Offer() { Odd = 1.22, OfferId = 502570803 },
                    new API_BET.Dal.Contract.Offer() { Odd = 1.10, OfferId = 502570804 }
                },
                TotalOdds = 1.342
            };
        }
        public static BetRequest DummyBetRequestInvalidStake()
        {
            var request = DummyBetRequest();
            request.Stake = 0;
            return request;
        }

        public static BetRequest DummyBetRequestInvalidCustomerID()
        {
            var request = DummyBetRequest();
            request.CustomerId = null;

            return request;
        }

        public static InvalidBetPlacementRequestException DummyInvalidBetException()
        {
            return new InvalidBetPlacementRequestException("reason");
        }
        public static Exception DummyAnyException()
        {
            return new Exception("reason");
        }
    }

}
