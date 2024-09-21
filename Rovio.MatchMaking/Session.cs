namespace Rovio.MatchMaking;

public class Session
{
    public Guid SessionId { get; set; }
    public IEnumerable<Player> Players { get; set; }
}