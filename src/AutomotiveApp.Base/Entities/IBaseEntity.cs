namespace AutomotiveApp.Base.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
        void MarkUpdated();
    }
}