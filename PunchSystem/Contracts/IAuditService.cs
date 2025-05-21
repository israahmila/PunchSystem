namespace PunchSystem.Contracts
{
    public interface IAuditService
    {
        Task LogAsync(string module, string action, string reference, string utilisateur, string raison);
    }
}
