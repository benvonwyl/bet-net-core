namespace API_BET.Dal.Settings
{
    public interface IBetDatabaseSettings
    {
        string BetCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}