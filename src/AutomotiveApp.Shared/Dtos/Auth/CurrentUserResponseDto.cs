namespace AutomotiveApp.Shared.Dtos.Auth
{
    public class CurrentUserResponseDto : IDto
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
