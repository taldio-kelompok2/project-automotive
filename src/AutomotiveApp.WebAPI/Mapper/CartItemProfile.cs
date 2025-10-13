using AutoMapper;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Shared.Dtos.CartItems;
using AutomotiveApp.Shared.Dtos.Carts;

namespace AutomotiveApp.WebAPI.Mapper
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemReadDto>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Session.Course))
                .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => src.Session.Date));

            CreateMap<CartItemCreateDto, CartItem>();
        }
    }
}