namespace PruebaRedsoft.Models
{
    public interface IPolicyStoreDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string PolicysCollectionName { get; set; }

    }
}
