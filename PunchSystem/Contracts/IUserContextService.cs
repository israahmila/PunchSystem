namespace PunchSystem.Contracts
{
    public interface IUserContextService
    {
        string? GetCurrentUserId();
        string? GetCurrentUsername();
    }
}
