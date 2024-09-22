using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Rovio.MatchMaking;
namespace Rovio.MatchMaking.Net;

[Route("match")]
public class MatchMakingController : Controller
{
    // Assuming there's already an injected service/repository to handle queuing
    private readonly IQueuedPlayerRepository _queuedPlayerRepository;
    public MatchMakingController(IQueuedPlayerRepository queuedPlayerRepository)
    {
        _queuedPlayerRepository = queuedPlayerRepository;
    }

    [HttpPost("queue")]
    public async Task<IActionResult> QueuePlayerAsync([FromBody] Player player)
    {
        // throw new NotImplementedException();
        if (player == null)
        {
            return BadRequest(new { error = "Player cannot be null" });
        }

        // Check if the player is already queued
        var existingQueuedPlayer = await _queuedPlayerRepository.GetQueuedPlayerByPlayerIdAsync(player.Id);
        if (existingQueuedPlayer != null)
        {
            return BadRequest(new { error = "Player is already queued" });
        }

        // Log Player properties to the console
        Console.WriteLine($"Player Queued: ID = {player.Id}, Name = {player.Name}, Latency = {player.LatencyMilliseconds}");
        
        var queuedPlayer = QueuedPlayer.CreateQueuedPlayerFromPlayer(player);
        var createdQueuedPlayer = await _queuedPlayerRepository.CreateQueuedPlayerAsync(queuedPlayer);
        // Assuming the sessionFactory handles the logic for queuing a player
        // await _sessionFactory.QueuePlayerAsync(player);
        // await _sessionFactory.Create();

        return Ok(new { message = "Player queued successfully" });
    }
    
    [HttpPost("dequeue")]
    public async Task<IActionResult> DequeuePlayerAsync([FromBody] Player player)
    {
        if (player == null)
        {
            return BadRequest(new { error = "Player cannot be null" });
        }

        // Check if the player is already queued
        var queuedPlayer = await _queuedPlayerRepository.GetQueuedPlayerByPlayerIdAsync(player.Id);
        if (queuedPlayer == null)
        {
            return BadRequest(new { error = "Player is not queued" });
        }

        await _queuedPlayerRepository.DeleteQueuedPlayerAsync(queuedPlayer.Id);

        return Ok(new { message = "Player dequeued successfully" });
    }
}