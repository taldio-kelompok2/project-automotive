using System.Security.Claims;

namespace AutomotiveApp.BlazorUI.Models.Auth.Context
{
    public class UserContextService(ILogger<UserContextService> logger)
    {
        private readonly ILogger<UserContextService> _logger = logger;

        public UserContext Current { get; private set; } = new UserContext();

        public void Update(string accessToken)
        {
            var oldContext = Current;
            Current = Current.Update(accessToken);
            _logger.LogInformation("UserContext updated. Old: {@OldContext}, New: {@NewContext}", oldContext.AccessToken, Current.AccessToken);
        }

        public void Clear()
        {
            _logger.LogInformation("UserContext cleared. Previous: {@OldContext}", Current.AccessToken);

            Current = UserContext.Guest;
        }
    }
}