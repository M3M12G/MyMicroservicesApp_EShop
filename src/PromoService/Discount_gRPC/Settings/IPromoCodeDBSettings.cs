namespace Discount_gRPC.Settings
{
    public interface IPromoCodeDBSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
