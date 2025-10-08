namespace AutomotiveApp.Shared.Models
{
    public record PaginatedResult<T>(IEnumerable<T> Items, int TotalCount);

}