using MongoDB.Driver;
using Promocode.API.Entities;

namespace Promocode.API.Data.Interfaces
{
    public interface IPromocodeContext
    {
        IMongoCollection<PromoCode> Promocodes { get; }
    }
}
