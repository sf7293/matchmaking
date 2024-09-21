using FluentAssertions;
using Xunit;

namespace Rovio.MatchMaking.Net.Tests;

public class MatchMakingControllerTests
{
    private readonly MatchMakingController _controller = new(); 
    
    [Fact]
    // MethodName_StateUnderTest_ExpectedBehavior
    public async Task QueuePlayerAsync_ExampleQueuePlayer_AddsPlayerToSession()
    {
        // 1. Arrange
        var player = new Player { Id = Guid.NewGuid(), LatencyMilliseconds = 100 };
        
        // 2. Act
        var result = await _controller.QueuePlayerAsync(player);

        // 3. Assert
        result.Should().NotBeNull();
    }
}