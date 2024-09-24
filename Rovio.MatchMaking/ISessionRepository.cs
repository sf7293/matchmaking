namespace Rovio.MatchMaking;

public interface ISessionRepository
{
    Task<IEnumerable<Session>> GetAllActiveSessionsAsync();
    Task<Session> CreateNewAsync(int latencyLevel, int joinedCount, int gameTimeInMinutes);
    Task<SessionPlayer> AddPlayerToSessionAsync(Guid sessionId, Guid playerId);
    Task<SessionPlayer> RemovePlayerFromSessionAsync(Guid sessionId, Guid playerId);
}