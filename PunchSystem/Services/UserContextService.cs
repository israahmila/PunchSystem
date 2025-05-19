namespace PunchSystem.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserContextService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string? GetCurrentUserId()
        {
            return _httpContext.HttpContext?.User?.FindFirst("UserId")?.Value;
        }

        public string? GetCurrentUsername()
        {
            return _httpContext.HttpContext?.User?.Identity?.Name;
        }
    }
    
    
}
