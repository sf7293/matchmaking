namespace Rovio.MatchMaking;

public interface ISessionRepository
{
    Task<Session> CreateNewAsync(int latencyLevel, int joinedCount, int gameTimeInMinutes);
    
    Task<Session> AddPlayerToSessionAsync(Player player);
    
    Task<Session> RemovePlayerFromSessionAsync(Player player);
}