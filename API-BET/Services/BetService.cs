using API_BET.Dal;
using API_BET.Model;
using API_BET.Services.Contract;
using API_BET.Services.Contract.Mapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetRequest = API_BET.Services.Contract.BetRequest;
using Offer = API_BET.Services.Contract.Offer;
using OfferRequest = API_BET.Services.Contract.OfferRequest;

namespace API_BET.Services
{
    public class BetService : IBetService
    {

        private readonly ILogger<IBetService> _logger;

        private readonly IOfferClient _offerClient;

        private readonly IBetDatabase _betDb;


        public BetService(ILogger<IBetService> logger, IOfferClient offerClient, IBetDatabase betDb)
        {
            _offerClient = offerClient;
            _logger = logger;
            _betDb = betDb;
        }

        public async Task<Bet> PlaceBet(BetRequest betRequest)
        {
            Bet bet = new Bet();

            bet.Stake = GetAndValidateStake(betRequest.Stake);
            bet.CustomerId = GetAndValidateCustomerId(betRequest.CustomerId);
            ValidatOffers(betRequest.Offers);

            bet.Offers = await CheckAndUpdateOdds(betRequest.Offers);
            bet.TotalOdds = ComputeAndValidateTotalOdds(bet.Offers);

            bet.PotentialWinning = ComputeAndValidatePotentialWinnings(bet.Stake, bet.TotalOdds);

            return RecordBetAndReturnRecord(bet);
        }

        public Bet GetBet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidBetIdRequestException("Invalid bet id format");
            }

            Dal.Contract.Bet bet;

            try
            {
                bet = _betDb.Get(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Id : {id}", id);
                throw new TechnicalException("Technical error trying to retrieve bet");
            }
            return bet.ToBetService();
        }

        private Bet RecordBetAndReturnRecord(Bet bet)
        {
            try
            {
                return _betDb.Create(bet.ToBetDB()).ToBetService();

            }
            catch (Exception e)
            {
                _logger.LogError(e,"Bet: {bet}", bet.ToString());
                throw new TechnicalException("Technical error trying to record bet ");
            }

        }

        private async Task<List<Offer>> CheckAndUpdateOdds(List<OfferRequest> offers)
        {
            List<Offer> response = new List<Offer>();
            List<OfferModel> liveOffers;
            try
            {
                liveOffers = await _offerClient.getOffers(offers.Select(t => t.OfferId.Value).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception("Technical error trying to retrieve offers "); // handled as 500 
            }

            foreach (var offer in offers)
            {
                OfferModel freshOffer = liveOffers?.Where(t => t.Identifier.Id == offer.OfferId && t.Status == 1)?.FirstOrDefault();

                if (freshOffer == null)
                {
                    throw new InvalidBetPlacementRequestException(" Unable To Find active offer " + offer.OfferId);
                }

                if (offer.Odd <= 0.90 * freshOffer.Odds || offer.Odd > freshOffer.Odds)
                {
                    throw new InvalidBetPlacementRequestException(" Unable To validate offer's odd " + offer.OfferId);
                }

                response.Add(new Offer() { Odd = freshOffer.Odds, OfferId = freshOffer.Identifier.Id });
            }
            return response;
        }

        private double ComputeAndValidateTotalOdds(List<Offer> offers)
        {
            double totalOdds = offers.Select(t => t.Odd).Aggregate((t, u) => t * u);

            if (totalOdds >= 20000 || totalOdds < 1.1)
            {
                throw new InvalidBetPlacementRequestException("Odds shall be greater or equal than 1.1 and lesser or equal 20000");
            }
            return totalOdds;
        }

        private double GetAndValidateStake(double? stake)
        {
            if (stake == null || stake <= 0)
            {
                throw new InvalidBetPlacementRequestException("Invalid Stake amount");
            }
            return stake.Value;
        }

        private static Guid GetAndValidateCustomerId(Guid? customerId)
        {
            if (customerId == null)
            {
                throw new InvalidBetPlacementRequestException("Invalid Customer Id");
            }

            return customerId.Value;
        }

        private double ComputeAndValidatePotentialWinnings(double stake, double totalOdds)
        {
            double potentialWinnings = stake * totalOdds;

            if (potentialWinnings >= 50000)
            {
                throw new InvalidBetPlacementRequestException("Invalid Potential Winning Amount");
            }
            return potentialWinnings;
        }
        private void ValidatOffers(List<OfferRequest> offers)
        {
            if (offers == null || !offers.Any())
            {
                throw new InvalidBetPlacementRequestException("Offers cannot be empty");
            }
            if (offers.Any(o => o.Odd == null || o.Odd <= 1))
            {
                throw new InvalidBetPlacementRequestException("Offers must have a valid Odd");
            }
            if (offers.Any(o => o.OfferId == null || o.OfferId <= 0))
            {
                throw new InvalidBetPlacementRequestException("Offers must have a valid OfferId");
            }
        }
    }
}
