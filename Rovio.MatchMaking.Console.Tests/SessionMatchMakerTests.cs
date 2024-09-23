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
        public async Task CreatePlayersMapAsync_ShouldReturnCorrectMap()
        {
            // Arrange
            var players = new List<QueuedPlayer>
            {
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 1, CreatedAt = DateTime.UtcNow },
                new QueuedPlayer { PlayerId = Guid.NewGuid(), LatencyLevel = 2, CreatedAt = DateTime.UtcNow }
            };
            await _context.QueuedPlayers.AddRangeAsync(players);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sessionMatchMaker.CreateQueuedPlayersMapAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(1, result.Keys);
            Assert.Contains(2, result.Keys);
        }

    }
}
