using MongoDB.Driver;
using Promocode.API.Data.Interfaces;
using Promocode.API.Entities;
using Promocode.API.Settings;

namespace Promocode.API.Data
{
    public class PromocodeContext : IPromocodeContext
    {
        public PromocodeContext(IPromocodeDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Promocodes = database.GetCollection<PromoCode>(settings.CollectionName);
            PromocodeContextSeed.SeedData(Promocodes);
        }
        public IMongoCollection<PromoCode> Promocodes { get; }
    }
}
