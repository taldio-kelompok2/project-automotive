using AutoMapper;
using AutomotiveApp.Domain.Entities.Invoices;
using AutomotiveApp.Shared.Dtos.User;

namespace AutomotiveApp.Application.Mapper
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, DashboardTransactionDto>()
                .ForMember(dest => dest.InvoiceCode, opt => opt.MapFrom(src => src.InvoiceCode))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Order != null && src.Order.User != null
                        ? src.Order.User.Email
                        : string.Empty))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Order != null && src.Order.User != null
                        ? src.Order.User.UserName
                        : string.Empty))
                .ForMember(dest => dest.PaymentMethodName,
                    opt => opt.MapFrom(src => src.Order != null && src.Order.PaymentMethod != null
                        ? src.Order.PaymentMethod.Name
                        : string.Empty))
                .ForMember(dest => dest.CourseCount, opt => opt.Ignore());
        }
    }
}
