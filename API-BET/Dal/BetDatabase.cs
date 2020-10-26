using API_BET.Dal.Settings;
using API_BET.Dal.Contract;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace API_BET.Dal
{
    public class BetDatabase : IBetDatabase
    {
        private readonly IMongoCollection<Bet> _bets;

        public BetDatabase(IBetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _bets = database.GetCollection<Bet>(settings.BetCollectionName);
        }

        public List<Bet> Get() =>
            _bets.Find(Bet => true).ToList();

        public Bet Get(string id) =>
            _bets.Find<Bet>(Bet => Bet.Id == id).FirstOrDefault();

        public Bet Create(Bet Bet)
        {
            _bets.InsertOne(Bet);
            return Bet;
        }

        public void Update(string id, Bet BetIn) =>
            _bets.ReplaceOne(Bet => Bet.Id == id, BetIn);

        public void Remove(Bet BetIn) =>
            _bets.DeleteOne(Bet => Bet.Id == BetIn.Id);

        public void Remove(string id) =>
            _bets.DeleteOne(Bet => Bet.Id == id);
    }
}