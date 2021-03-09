using AutoMapper;
using ShoppingWeb.Models;

namespace ShoppingWeb.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PromoCodeResponse, PromoCodeEntity>().ReverseMap();
        }
    }
}
