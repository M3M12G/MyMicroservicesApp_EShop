using Discount_gRPC.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discount_gRPC.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<PromoCode>> GetAllPromocodesAsync();
        Task<IEnumerable<PromoCode>> GetAllValidPromocodesAsync();
        Task<IEnumerable<PromoCode>> GetPromocodesByTitleAsync(string title);
        Task<PromoCode> GetPromoCode(string code);
        Task<PromoCode> GetPromocodeByIdAsync(string id);
        Task CreateAsync(PromoCode code);
        Task CreateManyAsync(IEnumerable<PromoCode> promo_codes);
        Task<bool> UpdateAsync(PromoCode activation_code);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteInvalidPromoAsync();
    }
}
