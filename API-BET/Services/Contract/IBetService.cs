using API_BET.Model;
using API_BET.Services.Contract;
using System.Threading.Tasks;
using BetRequest = API_BET.Services.Contract.BetRequest;

namespace API_BET.Services
{
    public interface IBetService
    {
        Task<Bet> PlaceBet(BetRequest betRequest);
        Bet GetBet(string id);
    }
}