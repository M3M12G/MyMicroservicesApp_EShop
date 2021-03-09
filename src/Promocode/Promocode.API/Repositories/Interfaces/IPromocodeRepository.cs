using Promocode.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promocode.API.Repositories.Interfaces
{
    public interface IPromocodeRepository
    {
        Task<IEnumerable<PromoCode>> GetAllPromocodesAsync();
        Task<IEnumerable<PromoCode>> GetAllValidPromocodesAsync();
        Task<IEnumerable<PromoCode>> GetPromocodesByTargetAsync(string target);
        Task<IEnumerable<PromoCode>> GetPromocodesByTitleAsync(string title);
        Task<PromoCode> GetPromocodeAsync(string id);
        Task CreateAsync(PromoCode code);
        Task CreateManyAsync(IEnumerable<PromoCode> promo_codes);
        Task<bool> UpdateAsync(PromoCode activation_code);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteInvalidPromoAsync();
    }
}
