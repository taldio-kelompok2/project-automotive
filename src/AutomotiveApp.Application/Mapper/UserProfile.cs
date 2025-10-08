using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;

namespace AutomotiveApp.Application.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserQueryDto>()
            .ForMember(dest => dest.BookingsCount, opt => opt.MapFrom(src => src.Bookings.Count()))
            .ForMember(dest => dest.OrdersCount, opt => opt.MapFrom(src => src.Orders.Count()))
            .ForMember(dest => dest.HasCart, opt => opt.MapFrom(src => src.Cart != null ));

            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
