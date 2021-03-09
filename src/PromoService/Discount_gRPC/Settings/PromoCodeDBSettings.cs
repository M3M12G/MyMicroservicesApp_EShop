namespace Discount_gRPC.Settings
{
    public class PromoCodeDBSettings : IPromoCodeDBSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
