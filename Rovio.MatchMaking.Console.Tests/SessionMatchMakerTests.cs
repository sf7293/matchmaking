using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rovio.MatchMaking.Console.Services;
using Rovio.MatchMaking.Repositories;
using Rovio.MatchMaking.Repositories.Data;
using Xunit;

namespace Rovio.MatchMaking.Console.Tests
{
    public class SessionMatchMakerTests
    {
        private readonly Mock<IQueuedPlayerRepository> _queuedPlayerRepositoryMock;
        private readonly Mock<ISessionRepository> _sessionRepositoryMock;
        private readonly AppDbContext _context;
        private readonly SessionMatchMaker _sessionMatchMaker;

        public SessionMatchMakerTests()
        {
            // Create an in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _queuedPlayerRepositoryMock = new Mock<IQueuedPlayerRepository>();
            _sessionRepositoryMock = new Mock<ISessionRepository>();

            // Initialize the SessionMatchmaker with mocked dependencies
            _sessionMatchMaker = new SessionMatchMaker(
                _context,
                _queuedPlayerRepositoryMock.Object,
                _sessionRepositoryMock.Object
            );
        }

        [Fact]
        public async Task CreatePlayersMapAsync_ShouldReturnCorrectMap1()
        {
            // Arrange
            var players = new List<QueuedPlayer>
            {
                new QueuedPlayer { Id = Guid.NewGuid(), PlayerId = Guid.NewGuid(), LatencyLevel = 1, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { Id = Guid.NewGuid(), PlayerId = Guid.NewGuid(), LatencyLevel = 2, CreatedAt = DateTime.UtcNow }
            };
            _queuedPlayerRepositoryMock.Setup(qpr => qpr.GetAllQueuedPlayersAsync()).ReturnsAsync(players);

            // Act
            var result = await _sessionMatchMaker.CreateQueuedPlayersMapAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(1, result.Keys);
            Assert.Equal(1, result[1].Count);
            Assert.Equal(players[0].PlayerId, result[1][0].PlayerId);
            Assert.Contains(2, result.Keys);
            Assert.Equal(1, result[2].Count);
            Assert.Equal(players[1].PlayerId, result[2][0].PlayerId);
        }

        [Fact]
        public async Task CreatePlayersMapAsync_ShouldReturnCorrectMap2()
        {
            // Arrange
            var players = new List<QueuedPlayer>
            {
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 1, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 2, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 2, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 2, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 3, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 3, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 4, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 4, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 4, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 4, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 4, CreatedAt = DateTime.UtcNow },
            };
            _queuedPlayerRepositoryMock.Setup(qpr => qpr.GetAllQueuedPlayersAsync()).ReturnsAsync(players);

            // Act
            var result = await _sessionMatchMaker.CreateQueuedPlayersMapAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Contains(1, result.Keys);
            Assert.Equal(1, result[1].Count);
            Assert.Equal(players[0].PlayerId, result[1][0].PlayerId);
            Assert.Contains(2, result.Keys);
            Assert.Equal(3, result[2].Count);
            Assert.Equal(players[1].PlayerId, result[2][0].PlayerId);
            Assert.Equal(players[2].PlayerId, result[2][1].PlayerId);
            Assert.Equal(players[3].PlayerId, result[2][2].PlayerId);
        }
    }
}
