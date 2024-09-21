namespace Rovio.MatchMaking;

public interface ISessionRepository
{
    Task<Session> CreateNewAsync();
    
    Task<Session> AddPlayerToSessionAsync(Player player);
    
    Task<Session> RemovePlayerFromSessionAsync(Player player);
}