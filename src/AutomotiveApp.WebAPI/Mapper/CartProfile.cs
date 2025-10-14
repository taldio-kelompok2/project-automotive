using AutoMapper;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Shared.Dtos.Carts;

namespace AutomotiveApp.WebAPI.Mapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartReadDto>();
            CreateMap<Cart, CartReadDetailsDto>();
            CreateMap<CartCreateDto, Cart>()
            .ForMember(c => c.User, opt => opt.Ignore())
            .ForMember(c => c.Items, opt => opt.Ignore())
            .ForMember(c => c.TotalPrice, opt => opt.MapFrom(_ => 0));
        }
    }
}