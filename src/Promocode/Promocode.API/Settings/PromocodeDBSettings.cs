namespace Promocode.API.Settings
{
    public class PromocodeDBSettings : IPromocodeDBSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
