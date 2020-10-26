using API_BET.Services;
using Flurl.Http.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace API_BET_TESTS
{
    public class OffersClientTests
    {
        HttpTest _httpTest;

        public OffersClientTests()
        {
           _httpTest = new HttpTest();
        }

        
        [Fact]
        public async void Return_Result_When_Api_Answers()
        {
            var offerclient = new OfferClient();
            _httpTest.RespondWith(Mocks.DummyOfferApiResponse());

            List<long> request = new List<long>();
            request.Add(502570803);

            var res = await offerclient.getOffers(request);

            Assert.True(res != null, "return a result");
            Assert.True(res.Count() == 1);

            var offerRes = res.FirstOrDefault();

            Assert.Equal(502570803, offerRes.Identifier.Id);
            Assert.Equal(1.22, offerRes.Odds);
            Assert.Equal(1, offerRes.Status);

        }

        [Fact]
        public async void Null_When_Empty_Input()
        {
            var offerclient = new OfferClient();

            List<long> request = new List<long>();

            var x = await offerclient.getOffers(request);

            Assert.True(x == null, "return not any result");
        }

        [Fact]
        public void Dispose()
        {
            _httpTest.Dispose();
        }
    }
}
