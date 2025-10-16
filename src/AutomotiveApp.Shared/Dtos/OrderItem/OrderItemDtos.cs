using System;
using System.Diagnostics.Eventing.Reader;
using AutomotiveApp.Domain.Entities.Courses;
using Microsoft.AspNetCore.Localization;

namespace AutomotiveApp.Shared.Dtos.OrderItem
{
    public class OrderItemCreateDto : BaseCommandDto, IDto
    {
        public long Price { get; set; }
        public Guid OrderId { get; set; }
        public Guid SessionId { get; set; }
    }

    public class OrderItemUpdateDto : BaseCommandDto, IDto
    {
        public long Price { get; set; }
        public Guid SessionId { get; set; }
    }

    public class OrderItemReadDto : BaseQueryDto, IDto
    {
        public long Price { get; set; }
        public Guid OrderId { get; set; }
        public Guid SessionId { get; set; }
        public CourseSession Session { get; set; } = null!;

    }
}
