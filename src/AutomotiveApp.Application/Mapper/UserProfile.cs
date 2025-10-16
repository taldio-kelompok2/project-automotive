using AutoMapper;
using AutomotiveApp.Application.Orders;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.Carts;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Dtos.User;

namespace AutomotiveApp.Application.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // temp
            CreateMap<Order, OrderReadDto>();
            CreateMap<Cart, CartReadDto>();

            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.Orders,
                    opt => opt.MapFrom(src => src.Orders ?? new List<Order>()))
                .ForMember(dest => dest.Bookings,
                    opt => opt.MapFrom(src => src.Bookings ?? new List<CourseBooking>()))
                .ForMember(dest => dest.Cart,
                    opt => opt.MapFrom(src => src.Cart));

            CreateMap<User, UserQueryDto>();

            CreateMap<UserCreateRequestDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserUpdateRequestDto, User>()
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
