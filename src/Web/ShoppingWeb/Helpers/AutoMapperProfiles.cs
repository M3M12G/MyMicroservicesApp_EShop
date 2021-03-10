using AutoMapper;
using ShoppingWeb.Models;
using System;

namespace ShoppingWeb.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PromoCodeResponse, PromoCodeEntity>()
                .ForMember(pc => pc.ExpirationDate,
                opt => opt.MapFrom(src => src.ExpirationDate.ToDateTime()));
        }
    }
}
