using Rovio.MatchMaking.Repositories.Data;
namespace Rovio.MatchMaking.Repositories;

public class SessionFactory : ISessionFactory
{
    private readonly AppDbContext _context;

    public SessionFactory(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Session> Create()
    {
        var session = new Session
        {
            Id = Guid.NewGuid(),
            LatencyLevel = 1, // Default latency level
            JoinedCount = 0, // Default joined count
            CreatedAt = DateTime.UtcNow,
            StartsAt = DateTime.UtcNow.AddMinutes(5), // Example start time
            EndsAt = DateTime.UtcNow.AddMinutes(30) // Example end time
        };

        await _context.Sessions.AddAsync(session);
        await _context.SaveChangesAsync();

        return session;
    }
}