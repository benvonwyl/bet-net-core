using System;
using System.Threading.Tasks;
using API_BET.Services;
using API_BET.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_BET.Controllers
{
    [ApiController]
    [Route("bet")]
    public class BetController : ControllerBase
    {
        private ILogger<BetController> _logger;

        private readonly IBetService _betService;

        public BetController(ILogger<BetController> logger, IBetService betService)
        {
            _betService = betService;
            _logger = logger;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(String id)
        {
            Bet bet;
            try
            {
                bet = _betService.GetBet(id);
            }
            catch (InvalidBetIdRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e) 
            {
                return StatusCode(500, e); 
            }

            if (bet == null) 
            {
                return NotFound();
            }

            return Ok(bet);
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost]

        public async Task<ActionResult<Bet>> PlaceBet(BetRequest betRequest)
        {
            try
            {
                var bet = await _betService.PlaceBet(betRequest);

                return Created("", bet);
            }
            catch (InvalidBetPlacementRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
