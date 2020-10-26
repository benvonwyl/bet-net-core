using API_BET.Dal.Contract;
using System.Collections.Generic;

namespace API_BET.Dal
{
    public interface IBetDatabase
    {
        Bet Create(Bet Bet);
        List<Bet> Get();
        Bet Get(string id);
        void Remove(Bet BetIn);
        void Remove(string id);
        void Update(string id, Bet BetIn);
    }
}