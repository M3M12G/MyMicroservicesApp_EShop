namespace Promocode.API.Settings
{
    public interface IPromocodeDBSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
