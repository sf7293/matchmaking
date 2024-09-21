namespace Rovio.MatchMaking.Repositories;

public class SessionRepository : ISessionRepository
{
    public async Task<Session> CreateNewAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Session> AddPlayerToSessionAsync(Player player)
    {
        throw new NotImplementedException();
    }

    public async Task<Session> RemovePlayerFromSessionAsync(Player player)
    {
        throw new NotImplementedException();
    }
}