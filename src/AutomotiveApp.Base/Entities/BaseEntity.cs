namespace AutomotiveApp.Base.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
        public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;
    }
}