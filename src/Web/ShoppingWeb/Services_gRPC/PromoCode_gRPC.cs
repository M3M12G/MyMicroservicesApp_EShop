using AutoMapper;
using Grpc.Core;
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

        public async Task<IEnumerable<PromoCodeEntity>> GetAllValidPromos()
        {
            ICollection<PromoCodeEntity> promosToReturn = new LinkedList<PromoCodeEntity>();

            using (var call = _discountService.GetAllPromos(new AllPromoRequest { }))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var promoCodeMsg = call.ResponseStream.Current;
                    var mapped_pc = _mapper.Map<PromoCodeEntity>(promoCodeMsg);
                    promosToReturn.Add(mapped_pc);
                }
            }

            return promosToReturn;
        }

        public async Task<IEnumerable<PromoCodeEntity>> GetPromosByTitle(string title)
        {
            ICollection<PromoCodeEntity> promosByTitle = new LinkedList<PromoCodeEntity>();

            using (var call = _discountService.GetPromoByTitle(new PromoTitleRequest { Title = title }))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var promoCodeMsg = call.ResponseStream.Current;
                    var mapped_pc = _mapper.Map<PromoCodeEntity>(promoCodeMsg);
                    promosByTitle.Add(mapped_pc);
                }
            }

            return promosByTitle;
        }
    }
}
