using Discount_gRPC.Entites;
using MongoDB.Driver;

namespace Discount_gRPC.Data.Interfaces
{
    public interface IDiscountContext
    {
        IMongoCollection<PromoCode> Promocodes { get; }
    }
}
