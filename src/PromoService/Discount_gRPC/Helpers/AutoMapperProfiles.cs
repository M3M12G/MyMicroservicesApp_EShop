using AutoMapper;
using Discount_gRPC.Entites;

namespace Discount_gRPC.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PromoCode, PromoCodeResponse>()
                .ForMember(pc => pc.ExpirationDate, 
                opt => opt.MapFrom(src => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(src.ExpirationDate)));
        }
    }
}
