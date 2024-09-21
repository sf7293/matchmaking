using Microsoft.AspNetCore.Mvc;

namespace Rovio.MatchMaking.Net;

[Route("match")]
public class MatchMakingController : Controller
{
    [HttpPost("queue")]
    public async Task<Session> QueuePlayerAsync(Player player)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("dequeue")]
    public async Task DequeuePlayerAsync(Player player)
    {
        throw new NotImplementedException();
    }
}