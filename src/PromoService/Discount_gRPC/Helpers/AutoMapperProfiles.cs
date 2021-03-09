using AutoMapper;
using Discount_gRPC.Entites;

namespace Discount_gRPC.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PromoCodeResponse, PromoCode>().ReverseMap();
        }
    }
}
