using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Rovio.MatchMaking;
namespace Rovio.MatchMaking.Net;

[Route("match")]
public class MatchMakingController : Controller
{
    // Assuming there's already an injected service/repository to handle queuing
    private readonly ISessionFactory _sessionFactory;
    private readonly IQueuedPlayerRepository _queuedPlayerRepository;
    public MatchMakingController(ISessionFactory sessionFactory, IQueuedPlayerRepository queuedPlayerRepository)
    {
        _sessionFactory = sessionFactory;
        _queuedPlayerRepository = queuedPlayerRepository;
    }

    [HttpPost("queue")]
    public async Task<IActionResult> QueuePlayerAsync(Player player)
    {
        // throw new NotImplementedException();
            if (player == null)
            {
                return BadRequest("Player cannot be null");
            }

            // Log Player properties to the console
            Console.WriteLine($"Player Queued: ID = {player.Id}, Name = {player.Name}, Latency = {player.LatencyMilliseconds}");
            
            var queuedPlayer = QueuedPlayer.CreateQueuedPlayerFromPlayer(player);
            var createdQueuedPlayer = await _queuedPlayerRepository.CreateQueuedPlayerAsync(queuedPlayer);
            // Assuming the sessionFactory handles the logic for queuing a player
            // await _sessionFactory.QueuePlayerAsync(player);
            // await _sessionFactory.Create();

            return Ok("Player queued successfully");
    }
    
    [HttpPost("dequeue")]
    public async Task DequeuePlayerAsync(Player player)
    {
        throw new NotImplementedException();
    }
}