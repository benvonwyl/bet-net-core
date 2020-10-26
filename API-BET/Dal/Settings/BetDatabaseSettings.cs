namespace API_BET.Dal.Settings
{
    public class BetDatabaseSettings : IBetDatabaseSettings
    {
        public string BetCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
