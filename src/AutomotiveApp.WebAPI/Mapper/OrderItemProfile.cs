using AutoMapper;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Shared.Dtos.OrderItem;

namespace AutomotiveApp.WebAPI.Mapper
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemReadDto>();
        }
    }
}