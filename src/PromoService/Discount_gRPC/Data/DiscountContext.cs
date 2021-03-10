using Discount_gRPC.Data.Interfaces;
using Discount_gRPC.Entites;
using Discount_gRPC.Settings;
using MongoDB.Driver;
using System;

namespace Discount_gRPC.Data
{
    public class DiscountContext : IDiscountContext
    {
        public DiscountContext(IPromoCodeDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Promocodes = database.GetCollection<PromoCode>(settings.CollectionName);
            DiscountContextSeed.SeedData(Promocodes);
        }
        public IMongoCollection<PromoCode> Promocodes { get; }
    }
}
