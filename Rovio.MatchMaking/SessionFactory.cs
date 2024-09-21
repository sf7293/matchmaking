namespace Rovio.MatchMaking;

public class SessionFactory : ISessionFactory
{
    public Task<Session> Create()
    {
        Console.WriteLine("Hello");

    // Create a new Session object
    var session = new Session
    {
        // Initialize the Session object properties here
        SessionId = Guid.NewGuid(),
        // CreatedAt = DateTime.UtcNow,
        Players = new List<Player>() // Assuming Session has a Players property
    };

    // Return the session wrapped in a Task
    return Task.FromResult(session);
    }
}