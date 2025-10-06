using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AutomotiveApp.Shared.Dtos
{
    public interface IDto { }

    public interface IQueryDto : IDto
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }

    public interface ICommandDto : IDto
    {
        Guid Id { get; }
    }

    public abstract class BaseQueryDto : IQueryDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public abstract class BaseCommandDto : ICommandDto
    {
        [JsonIgnore]
        [SwaggerSchema(ReadOnly = true)]
        public Guid Id { get; } = Guid.NewGuid();
    }
}