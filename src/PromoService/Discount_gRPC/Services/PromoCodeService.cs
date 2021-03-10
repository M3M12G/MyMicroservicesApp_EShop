using AutoMapper;
using Discount_gRPC.Entites;
using Discount_gRPC.Helpers;
using Discount_gRPC.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discount_gRPC.Services
{
    public class PromoCodeService : PromoDiscountService.PromoDiscountServiceBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PromoCodeService> _logger;
        private readonly IDiscountRepository _repository;

        public PromoCodeService(IDiscountRepository repository, ILogger<PromoCodeService> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task GetAllPromos(AllPromoRequest request, IServerStreamWriter<PromoCodeResponse> responseStream, ServerCallContext context)
        {
            var allValidPromos = await _repository.GetAllValidPromocodesAsync();
            
            if(allValidPromos != null)
            {
                foreach(var singlePromo in allValidPromos)
                {
                    var promo = _mapper.Map<PromoCodeResponse>(singlePromo);

                    await responseStream.WriteAsync(promo);
                }
                
            }
        }

        public override async Task GetPromoByTitle(PromoTitleRequest request, IServerStreamWriter<PromoCodeResponse> responseStream, ServerCallContext context)
        {
            var promoForTitle = await _repository.GetPromocodesByTitleAsync(request.Title);

            if(promoForTitle == null)
            {
                _logger.LogWarning($"[PROMO-NOT FOUND]<>Not found any promo for {request.Title}");
                return;
            }

            foreach(var promo in promoForTitle)
            {
                var pT = _mapper.Map<PromoCodeResponse>(promo);

                await responseStream.WriteAsync(pT);
            }
        }

        public override async Task<ExecutionStatusResponse> CreatePromo(PromoCreateRequest request, ServerCallContext context)
        {
            var toSaveCode = new PromoCode()
            {
                Code = PromoGenerator.GenerateCode(),
                Title = request.Title,
                ExpirationDate = Convert.ToDateTime(request.ExpirationDate),
                Discount = request.Discount
            };

            var res = new ExecutionStatusResponse
            {
                Status = false
            };

            try
            {
                await _repository.CreateAsync(toSaveCode);
                res.Status = true;
                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"[PROMO-CREATION]<>Some errors occured: {e.Message}");
            }

            return res;
        }

        public override async Task<ExecutionStatusResponse> GeneratePromoCodes(GeneratePromoRequest request, ServerCallContext context)
        {
            List<PromoCode> promoCodesToSave = new List<PromoCode>();

            var genResult = new ExecutionStatusResponse() { Status = false };

            try
            {
                for (int i = 0; i < request.Quantity; i++)
                {
                    PromoCode pc = new PromoCode()
                    {
                        Code = PromoGenerator.GenerateCode(),
                        Title = request.Title,
                        ExpirationDate = Convert.ToDateTime(request.ExpirationDate),
                        Discount = request.Discount
                    };

                    promoCodesToSave.Add(pc);
                }

                await _repository.CreateManyAsync(promoCodesToSave);
                genResult.Status = true;
                return genResult;
            }
            catch (Exception e)
            {
                _logger.LogError($"[PROMO-GENERATION]<>Some errors occured: {e.Message}");
            }

            return genResult;
        }
        public override async Task<ExecutionStatusResponse> ActivatePromoCode(ActivationPromoRequest request, ServerCallContext context)
        {
            var activationRes = new ExecutionStatusResponse()
            {
                Status = false
            };

            try
            {
                var promoFromDB = await _repository.GetPromoCode(request.Code);
                if(promoFromDB != null)
                {
                    if(promoFromDB.IsValid && promoFromDB.ExpirationDate > DateTime.Now)
                    {
                        promoFromDB.Username = request.Username;
                        promoFromDB.IsValid = false;
                    }

                    await _repository.UpdateAsync(promoFromDB);
                    activationRes.Status = true;
                    return activationRes;

                }
            }
            catch(Exception e)
            {
                _logger.LogError($"[PROMO-ACTIVATION]<>Some errors occured: {e.Message}");
            }

            return activationRes;
        }

        public override async Task<ExecutionStatusResponse> DeleteInvalidPromos(DeleteInvalidPromosRequest request, ServerCallContext context)
        {
            var multiDeletionRes = new ExecutionStatusResponse()
            {
                Status = false
            };

            try
            {
                await _repository.DeleteInvalidPromoAsync();
            }
            catch(Exception e)
            {
                _logger.LogError($"[PROMO-MULTI_INVALID_DELETION]<>Some errors occured: {e.Message}");
            }
            return multiDeletionRes;
        }
    }
}
