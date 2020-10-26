using API_BET.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_BET.Services
{
    public interface IOfferClient
    {
        Task<List<OfferModel>> getOffers(List<long> offerId);
    }
}