public class QueuedPlayer
{
    public Guid Id { get; set; } // Unique identifier for each queued request
    public Guid PlayerId { get; set; } // ID of the player requesting to join a session
    public int LatencyLevel { get; set; } // Latency level of the player (1 to 5)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp when the record was created
}