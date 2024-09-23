using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Rovio.MatchMaking.Console.Services;
using Rovio.MatchMaking.Repositories;
using Rovio.MatchMaking.Repositories.Data;
using Xunit;

namespace Rovio.MatchMaking.Console.Tests
{
    public class SessionMatchmakerTests
    {
        private readonly Mock<IQueuedPlayerRepository> _queuedPlayerRepositoryMock;
        private readonly Mock<ISessionRepository> _sessionRepositoryMock;
        private readonly SessionMatchmaker _sessionMatchmaker;

        public SessionMatchmakerTests()
        {
            _queuedPlayerRepositoryMock = new Mock<IQueuedPlayerRepository>();
            _sessionRepositoryMock = new Mock<ISessionRepository>();
            
            // Assuming SessionMatchmaker takes repositories as constructor arguments
            _sessionMatchmaker = new SessionMatchmaker(
                new Mock<AppDbContext>().Object, // Use mock context if needed
                _sessionRepositoryMock.Object
            );
        }

        [Fact]
        public async Task RunAsync_ShouldHandleMatchmakingLogic()
        {
            // Arrange
            // Mock necessary methods and data for repositories
            _queuedPlayerRepositoryMock.Setup(repo => repo.GetQueuedPlayersAsync())
                .ReturnsAsync(new List<QueuedPlayer>
                {
                    new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 1 }
                });

            _sessionRepositoryMock.Setup(repo => repo.CreateNewAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Session { Id = Guid.NewGuid(), LatencyLevel = 1 });

            // Act
            await _sessionMatchmaker.RunAsync();

            // Assert
            _sessionRepositoryMock.Verify(repo => repo.CreateNewAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _queuedPlayerRepositoryMock.Verify(repo => repo.DeleteQueuedPlayerAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
