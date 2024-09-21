using Rovio.MatchMaking.Repositories.Data;
namespace Rovio.MatchMaking.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;
    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Session> CreateNewAsync(int latencyLevel, int joinedCount, int gameTimeInMinutes)
    {
        // Validation checks for the parameters
        if (latencyLevel < 1 || latencyLevel > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(latencyLevel), "Latency level must be between 1 and 5.");
        }

        if (joinedCount < 0 || joinedCount > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(joinedCount), "Joined count must be between 0 and 10.");
        }

        if (gameTimeInMinutes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gameTimeInMinutes), "Game time must be a positive number.");
        }

        var session = new Session
        {
            Id = Guid.NewGuid(),
            LatencyLevel = latencyLevel,
            JoinedCount = joinedCount,
            CreatedAt = DateTime.UtcNow,
            StartsAt = DateTime.UtcNow,
            EndsAt = DateTime.UtcNow.AddMinutes(gameTimeInMinutes)
        };

        await _context.Sessions.AddAsync(session);
        await _context.SaveChangesAsync();

        return session;
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