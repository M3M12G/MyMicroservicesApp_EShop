using AutoMapper;
using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingWeb.Services_gRPC
{
    public class PromoCode_gRPC
    {
        private readonly PromoDiscountService.PromoDiscountServiceClient _discountService;
        private readonly IMapper _mapper;

        public PromoCode_gRPC(PromoDiscountService.PromoDiscountServiceClient discountService, IMapper mapper)
        {
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> ActivatePromoCode(string username, string code)
        {
            var codeRequest = new ActivationPromoRequest { Code = code, Username = username };

            var res = await _discountService.ActivatePromoCodeAsync(codeRequest);

            return res.Status;
        }

        public async Task<bool> GeneratePromoCodes(PromoGenReqDTO genRequest)
        {
            var requestToGen = new GeneratePromoRequest { Quantity = genRequest.Quantity, Title = genRequest.Title,
                                                          ExpirationDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(genRequest.ExpirationDate),
                                                          Discount = genRequest.Discount};

            var result = await _discountService.GeneratePromoCodesAsync(requestToGen);

            return result.Status;
        }

        public async Task<IEnumerable<PromoCodeEntity>> GetAllPromoCodes()
        {
            var promos = await _discountService.GetAllPromosAsync(new AllPromoRequest { });
            
            if(promos.PromoCodes.Count > 0)
            {
                var promosToReturns = _mapper.Map<IEnumerable<PromoCodeEntity>>(promos.PromoCodes);
                return promosToReturns;
            }

            return null;
        }
    }
}
