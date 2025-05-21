namespace PunchSystem.Contracts
{
    public interface IStatsService
    {
        Task<object> GetUtilisationsStatsAsync();
        Task<object> GetEtatPoinconsAsync();
        Task<object> GetGlobalStatsAsync();
    }

}
