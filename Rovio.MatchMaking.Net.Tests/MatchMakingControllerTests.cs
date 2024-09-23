using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Rovio.MatchMaking.Net;
using Rovio.MatchMaking;
using Xunit;

namespace Rovio.MatchMaking.Net.Tests
{
    public class MatchMakingControllerTests
    {
        private readonly Mock<IQueuedPlayerRepository> _queuedPlayerRepositoryMock;
        private readonly MatchMakingController _controller;
        private readonly Mock<ILogger<MatchMakingController>> _loggerMock;

        public MatchMakingControllerTests()
        {
            _queuedPlayerRepositoryMock = new Mock<IQueuedPlayerRepository>();
            _loggerMock = new Mock<ILogger<MatchMakingController>>();
            _controller = new MatchMakingController(_queuedPlayerRepositoryMock.Object);
        }

        [Fact]
        public async Task QueuePlayerAsync_ShouldReturnBadRequest_WhenPlayerIsNull()
        {
            // Act
            var result = await _controller.QueuePlayerAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Contains("Player cannot be null", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task QueuePlayerAsync_ShouldReturnOk_WhenPlayerIsValid()
        {
            // Arrange
            var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer", LatencyMilliseconds = 50 };
            var _queuedPlayer = QueuedPlayer.CreateQueuedPlayerFromPlayer(player);
            _queuedPlayerRepositoryMock.Setup(qpr => qpr.CreateQueuedPlayerAsync(_queuedPlayer)).ReturnsAsync(new QueuedPlayer());

            // Act
            var result = await _controller.QueuePlayerAsync(player);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Player queued successfully", okResult.Value.ToString());
        }

        [Fact]
        public async Task QueuePlayerAsync_ShouldReturnBadRequest_WhenPlayerHadBeenQueued()
        {
            // Arrange
            var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer", LatencyMilliseconds = 50 };
            var _queuedPlayer = QueuedPlayer.CreateQueuedPlayerFromPlayer(player);
            _queuedPlayerRepositoryMock.Setup(qpr => qpr.GetQueuedPlayerByPlayerIdAsync(player.Id)).ReturnsAsync(new QueuedPlayer());
            _queuedPlayerRepositoryMock.Setup(qpr => qpr.CreateQueuedPlayerAsync(_queuedPlayer)).ReturnsAsync(new QueuedPlayer());

            // Act
            var result = await _controller.QueuePlayerAsync(player);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Contains("Player is already queued", badRequestResult.Value.ToString());
        }

        //INJA: develop tests for dequeueing
    }
}
