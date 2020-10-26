using API_BET.Model;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_BET.Services
{
    public class OfferClient : IOfferClient
    {

        private static readonly string _baseURL = "https://offer.cdn.betclic.fr/api/pub/v2/selections?application=2&countrycode=fr&language=fr&sitecode=frfr";


        public OfferClient()
        {
        }

        public async Task<List<OfferModel>> getOffers(List<long> offerId)
        {
            if (offerId != null && offerId.Any())
            {
                string requestUrl = _baseURL + "&preLiveIds=" + string.Join(',', offerId) + "&liveIds=" + string.Join(',', offerId);
                return await requestUrl.GetJsonAsync<List<OfferModel>>();
            }
            return null;
        }
    }
}
