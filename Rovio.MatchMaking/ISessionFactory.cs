namespace Rovio.MatchMaking;

public interface ISessionFactory
{
    Task<Session> Create();
}