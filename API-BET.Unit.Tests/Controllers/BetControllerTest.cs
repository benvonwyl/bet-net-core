using API_BET.Controllers;
using API_BET.Model;
using API_BET.Services;
using API_BET.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace API_BET_TESTS
{
    public class BetControllerTest
    {

        [Fact]
        public async Task Post_SuccessFulls_201_When_Service_Returns_Value()
        {
            // Arrange
            var betService = new Mock<IBetService>();

            BetRequest betRequest = Mocks.DummyBetRequest();
            Bet betMock = new Bet();
            betService.Setup(x => x.PlaceBet(It.IsAny<BetRequest>())).ReturnsAsync(betMock);

            var betController = new BetController(null, betService.Object);

            // Act
            var result = await betController.PlaceBet(betRequest);

            // Assert
            Assert.IsType<ActionResult<Bet>>(result);
            Assert.IsType<CreatedResult>(result.Result);
            Assert.Same(((CreatedResult)result.Result).Value, betMock);
        }

        [Fact]
        public async Task Post_KO_400_When_Service_Returns_InvalidBetException()
        {
            // Arrange
            var betService = new Mock<IBetService>();

            BetRequest betRequest = Mocks.DummyBetRequest();
            Bet betMock = new Bet();

            betService.Setup(x => x.PlaceBet(It.IsAny<BetRequest>())).ThrowsAsync(Mocks.DummyInvalidBetException());
            var betController = new BetController(null, betService.Object);

            // Act
            var result = await betController.PlaceBet(betRequest);

            // Assert
            Assert.IsType<ActionResult<Bet>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Same(((BadRequestObjectResult)result.Result).Value, Mocks.DummyInvalidBetException().Message);
        }

        [Fact]
        public async Task Post_KO_500_When_Service_Returns_Any_Exception()
        {
            // Arrange
            var betService = new Mock<IBetService>();

            BetRequest betRequest = Mocks.DummyBetRequest();
            Bet betMock = new Bet();

            betService.Setup(x => x.PlaceBet(It.IsAny<BetRequest>())).ThrowsAsync(Mocks.DummyAnyException());
            var betController = new BetController(null, betService.Object);

            // Act
            var result = await betController.PlaceBet(betRequest);

            // Assert
            Assert.IsType<ActionResult<Bet>>(result);
            Assert.IsType<ObjectResult>(result.Result);
            Assert.Same(((ObjectResult)result.Result).Value, Mocks.DummyInvalidBetException().Message);
            Assert.Equal(500, ((ObjectResult)result.Result).StatusCode);
        }
    }
}
