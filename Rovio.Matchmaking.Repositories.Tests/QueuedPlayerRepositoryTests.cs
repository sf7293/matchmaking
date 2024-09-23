using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rovio.MatchMaking.Repositories.Data;
using Rovio.MatchMaking.Repositories;
using Xunit;

namespace Rovio.MatchMaking.Tests
{
    public class QueuedPlayerRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly IQueuedPlayerRepository _repository;

        public QueuedPlayerRepositoryTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new QueuedPlayerRepository(_context);
        }

        [Fact]
        public async Task AddQueuedPlayerAsync_ShouldAddPlayer()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var queuedPlayer = new QueuedPlayer { PlayerId = playerId, LatencyLevel = 1, CreatedAt = DateTime.UtcNow };

            // Act
            var insertedQueuedPlayer = await _repository.CreateQueuedPlayerAsync(queuedPlayer);
            await _context.SaveChangesAsync();
            var result = await _context.QueuedPlayers.FindAsync(insertedQueuedPlayer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(playerId, result.PlayerId);
        }

        [Fact]
        public async Task DeleteQueuedPlayerAsync_ShouldRemovePlayer()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var queuedPlayer = new QueuedPlayer { PlayerId = playerId, LatencyLevel = 1, CreatedAt = DateTime.UtcNow };
            await _context.QueuedPlayers.AddAsync(queuedPlayer);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteQueuedPlayerAsync(playerId);
            await _context.SaveChangesAsync();
            var result = await _context.QueuedPlayers.FindAsync(playerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateQueuedPlayerAsync_ShouldUpdatePlayer()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var queuedPlayer = new QueuedPlayer { PlayerId = playerId, LatencyLevel = 1, CreatedAt = DateTime.UtcNow };

            // Act
            var insertedQueuedPlayer = await _repository.CreateQueuedPlayerAsync(queuedPlayer);
            await _context.SaveChangesAsync();
            insertedQueuedPlayer.LatencyLevel = 2;
            _context.QueuedPlayers.Update(insertedQueuedPlayer);
            await _context.SaveChangesAsync();
            var result = await _context.QueuedPlayers.FindAsync(insertedQueuedPlayer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.LatencyLevel);
        }

        [Fact]
        public async Task GetQueuedPlayerByIdAsync_ShouldGetPlayer()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var queuedPlayer = new QueuedPlayer { PlayerId = playerId, LatencyLevel = 1, CreatedAt = DateTime.UtcNow };

            // Act
            var insertedQueuedPlayer = await _repository.CreateQueuedPlayerAsync(queuedPlayer);
            await _context.SaveChangesAsync();

            var result = await _repository.GetQueuedPlayerByIdAsync(insertedQueuedPlayer.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetQueuedPlayerByPlayerIdAsync_ShouldGetPlayer()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var queuedPlayer = new QueuedPlayer { PlayerId = playerId, LatencyLevel = 1, CreatedAt = DateTime.UtcNow };

            // Act
            var insertedQueuedPlayer = await _repository.CreateQueuedPlayerAsync(queuedPlayer);
            await _context.SaveChangesAsync();

            var result = await _repository.GetQueuedPlayerByPlayerIdAsync(insertedQueuedPlayer.PlayerId);

            // Assert
            Assert.NotNull(result);
        }
    }
}
