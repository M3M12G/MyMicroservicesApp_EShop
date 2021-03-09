using MongoDB.Driver;
using Promocode.API.Data.Interfaces;
using Promocode.API.Entities;
using Promocode.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promocode.API.Repositories
{
    public class PromocodeRepository : IPromocodeRepository
    {
        private readonly IPromocodeContext _context;

        public PromocodeRepository(IPromocodeContext promocodeContext)
        {
            _context = promocodeContext ?? throw new ArgumentNullException(nameof(promocodeContext));
        }

        public async Task CreateAsync(PromoCode code)
        {
            try
            {
                await _context.Promocodes.InsertOneAsync(code);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task CreateManyAsync(IEnumerable<PromoCode> promo_codes)
        {
            try
            {
                await _context.Promocodes.InsertManyAsync(promo_codes);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                FilterDefinition<PromoCode> filter = Builders<PromoCode>.Filter.Eq(pc => pc.Id, id);
                DeleteResult deleteResult = await _context
                                            .Promocodes
                                            .DeleteOneAsync(filter);

                return deleteResult.IsAcknowledged
                    && deleteResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> DeleteInvalidPromoAsync()
        {
            try
            {
                FilterDefinition<PromoCode> filter_by_valid = Builders<PromoCode>.Filter.Eq(pc => pc.IsValid, false);

                DeleteResult deleteResult = await _context
                                            .Promocodes
                                            .DeleteManyAsync(filter_by_valid);
                return deleteResult.IsAcknowledged
                        && deleteResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<PromoCode>> GetAllPromocodesAsync()
        {
            return await _context
                   .Promocodes
                   .Find(pc => true)
                   .SortByDescending(pc => pc.ExpirationDate)
                   .ToListAsync();
        }

        public async Task<IEnumerable<PromoCode>> GetAllValidPromocodesAsync()
        {
            FilterDefinition<PromoCode> filter_valids = Builders<PromoCode>.Filter.Eq(pc => pc.IsValid, true);

            return await _context
                        .Promocodes
                        .Find(filter_valids)
                        .SortByDescending(pc => pc.ExpirationDate)
                        .ToListAsync();
        }

        public async Task<PromoCode> GetPromocodeAsync(string id)
        {
            return await _context
                        .Promocodes
                        .Find(pc => pc.Id == id)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PromoCode>> GetPromocodesByTargetAsync(string target)
        {
            FilterDefinition<PromoCode> filter_by_target = Builders<PromoCode>.Filter.Eq(pc => pc.Target, target);

            return await _context
                         .Promocodes
                         .Find(filter_by_target)
                         .ToListAsync();
        }

        public async Task<IEnumerable<PromoCode>> GetPromocodesByTitleAsync(string title)
        {
            FilterDefinition<PromoCode> filter_by_title = Builders<PromoCode>.Filter.Eq(pc => pc.Title, title);

            return await _context
                         .Promocodes
                         .Find(filter_by_title)
                         .SortByDescending(pc => pc.ExpirationDate)
                         .ToListAsync();
        }

        public async Task<bool> UpdateAsync(PromoCode activation_code)
        {
            try
            {
                var updResult = await _context
                                  .Promocodes
                                  .ReplaceOneAsync(filter: pc => pc.Id == activation_code.Id, replacement: activation_code);

                return updResult.IsAcknowledged
                       && updResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> IsPromoValid(string id)
        {
            var promo = await GetPromocodeAsync(id);

            return promo.IsValid == true && promo.ExpirationDate > DateTime.Now;
        }
    }
}
