using API_BET.Dal;
using API_BET.Services;
using API_BET.Services.Contract;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace API_BET_TESTS
{
    public class BetServiceTest
    {

        /// <summary>
        /// One or multiple selection with identifier and odd
        /// A valid bet placement payload contains at least
        /// A customer identifier
        /// A stake amount 
        /// A valid bet placement response (when bet is successfully placed) contains at least 
        /// Bet identifier 
        /// Total odds of the bet (multiplication of all selections odds) 
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async Task PlaceBet_SuccessFull_Returns_Value()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var offerclientResponse = Mocks.DummyOfferClientResponse();

            var dbMock = new Mock<IBetDatabase>();
            var betDatabaseResponse = Mocks.BetDummyBetDatabaseResponse();


            offerClient.Setup(x => x.getOffers(It.IsAny<List<long>>())).ReturnsAsync(offerclientResponse);
            dbMock.Setup(x => x.Create(It.IsAny<API_BET.Dal.Contract.Bet>())).Returns(betDatabaseResponse);

            var betService = new BetService(null, offerClient.Object, dbMock.Object);
            var betRequest = Mocks.DummyBetRequest();

            //// Act
            var result = await betService.PlaceBet(betRequest);

            //// Assert
            Assert.True(result != null, "return a result");
            Assert.Equal(offerclientResponse.First().Identifier.Id, result.Offers.First().OfferId);
            Assert.Equal(offerclientResponse.First().Odds, result.Offers.First().Odd);
            Assert.Equal(betRequest.Stake, result.Stake);
            Assert.Equal(offerclientResponse.First().Odds * offerclientResponse[1].Odds, result.TotalOdds);
            Assert.Equal(betRequest.CustomerId, result.CustomerId);
            Assert.NotNull(result.Id);
        }

        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Invalid_Stake()
        {
            // Arrange

            var offerClient = new Mock<IOfferClient>();
            var betService = new BetService(null, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequestInvalidStake();

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));

            Assert.Equal("Invalid Stake amount", e.Message);
        }

        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Invalid_CustomerID()
        {
            // Arrange

            var offerClient = new Mock<IOfferClient>();
            var betService = new BetService(null, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequestInvalidCustomerID();

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));

            Assert.Equal("Invalid Customer Id", e.Message);
        }


        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Invalid_Offers()
        {
            // Arrange

            var offerClient = new Mock<IOfferClient>();
            var betService = new BetService(null, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();
            betRequest.Offers = null;

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));
            Assert.Equal("Offers cannot be empty", e.Message);
        }


        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Invalid_Offers_Odd()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var betService = new BetService(null, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();
            betRequest.Offers[0].Odd = null;

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));
            Assert.Equal("Offers must have a valid Odd", e.Message);
        }

        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Invalid_Offers_OfferId()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var betService = new BetService(null, offerClient.Object, null);


            var betRequest = Mocks.DummyBetRequest();
            betRequest.Offers[0].OfferId = null;

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));
            Assert.Equal("Offers must have a valid OfferId", e.Message);
        }

        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Offers_Client_Error()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var logger = new Mock<ILogger<IBetService>>();
            offerClient.Setup(x => x.getOffers(It.IsAny<List<long>>())).Throws(new Exception("reason"));

            var betService = new BetService(logger.Object, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();

            // Act - Assert 
            var e = await Assert.ThrowsAsync<Exception>(() => betService.PlaceBet(betRequest));
            Assert.Equal("Technical error trying to retrieve offers ", e.Message);
            //logger.Verify( x => x.LogError(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Offers_Doesnt_Exists()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var logger = new Mock<ILogger<IBetService>>();
            var offerClientsResponse = Mocks.DummyOfferClientResponse();
            offerClientsResponse.RemoveAt(1);

            offerClient.Setup(x => x.getOffers(It.IsAny<List<long>>())).ReturnsAsync(offerClientsResponse);

            var betService = new BetService(logger.Object, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));
            Assert.Equal(" Unable To Find active offer 502570804", e.Message);
        }
        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Offers_Odd_Too_Low()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var logger = new Mock<ILogger<IBetService>>();
            var offerClientsResponse = Mocks.DummyOfferClientResponse();
            offerClientsResponse[0].Odds *= 2;

            offerClient.Setup(x => x.getOffers(It.IsAny<List<long>>())).ReturnsAsync(offerClientsResponse);

            var betService = new BetService(logger.Object, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();
            
            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));
            Assert.Equal(" Unable To validate offer's odd 502570803", e.Message);
        }

        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Offers_Odd_Too_high()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var logger = new Mock<ILogger<IBetService>>();
            var offerClientsResponse = Mocks.DummyOfferClientResponse();
            offerClientsResponse[0].Odds *= 0.89;

            offerClient.Setup(x => x.getOffers(It.IsAny<List<long>>())).ReturnsAsync(offerClientsResponse);

            var betService = new BetService(logger.Object, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));
            Assert.Equal(" Unable To validate offer's odd 502570803", e.Message);
        }

        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Offers_PotentialWinning_Too_High()
        { 
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var logger = new Mock<ILogger<IBetService>>();
            var offerClientsResponse = Mocks.DummyOfferClientResponse();

            offerClient.Setup(x => x.getOffers(It.IsAny<List<long>>())).ReturnsAsync(offerClientsResponse);

            var betService = new BetService(logger.Object, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();
            betRequest.Stake = 50000;

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));
            Assert.Equal("Invalid Potential Winning Amount", e.Message);
        }
        
        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Offers_TotalOdds_Too_High()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var logger = new Mock<ILogger<IBetService>>();
            var offerClientsResponse = Mocks.DummyOfferClientResponse();
            offerClientsResponse[1].Odds *= 20000;

            offerClient.Setup(x => x.getOffers(It.IsAny<List<long>>())).ReturnsAsync(offerClientsResponse);

            var betService = new BetService(logger.Object, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();
            betRequest.Offers[1].Odd *= 20000;

            // Act - Assert 
            var e = await Assert.ThrowsAsync<InvalidBetIdRequestException>(() => betService.PlaceBet(betRequest));
            Assert.Equal("Odds shall be greater or equal than 1.1 and lesser or equal 20000", e.Message);
        }


        [Fact]
        public async Task PlaceBet_Throws_Exception_When_Offers_DB_Bet_Creation_KO()
        {
            // Arrange
            var offerClient = new Mock<IOfferClient>();
            var logger = new Mock<ILogger<IBetService>>();
            var offerClientsResponse = Mocks.DummyOfferClientResponse();

            var dbMock = new Mock<IBetDatabase>();

            dbMock.Setup(x => x.Create(It.IsAny<API_BET.Dal.Contract.Bet>())).Throws(new Exception("reason"));
            offerClient.Setup(x => x.getOffers(It.IsAny<List<long>>())).ReturnsAsync(offerClientsResponse);

            var betService = new BetService(logger.Object, offerClient.Object, null);
            var betRequest = Mocks.DummyBetRequest();

            // Act - Assert 
            var e = await Assert.ThrowsAsync<Exception>(() => betService.PlaceBet(betRequest));
            Assert.Equal("Technical error trying to record bet ", e.Message);
        }
    }
}
