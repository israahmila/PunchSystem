namespace PunchSystem.Services
{
    public interface IUserContextService
    {
        string? GetCurrentUserId();
        string? GetCurrentUsername();
    }
}
