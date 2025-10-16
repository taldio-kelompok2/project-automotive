using AutoMapper;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Shared.Dtos.Order;

namespace AutomotiveApp.WebAPI.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderReadDto>()
            .ForMember(src => src.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Name));

            CreateMap<Order, OrderReadDetailsDto>()
            .ForMember(src => src.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Name));
        }
    }
}