namespace PruebaRedsoft.Models
{
    public class PolicyStoreDatabaseSettings : IPolicyStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
        public string PolicysCollectionName { get; set; } = String.Empty;
    }
}
