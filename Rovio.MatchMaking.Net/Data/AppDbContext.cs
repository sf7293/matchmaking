using Microsoft.EntityFrameworkCore;
using Rovio.MatchMaking;

namespace Rovio.MatchMaking.Net.Data // Update the namespace as per your project structure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<QueuedPlayer> QueuedPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                        // Configuring GUID as CHAR(36)
            modelBuilder.Entity<Session>()
                .Property(e => e.SessionId)
                .HasColumnType("char(36)");

            modelBuilder.Entity<QueuedPlayer>()
                .Property(e => e.Id)
                .HasColumnType("char(36)");

            modelBuilder.Entity<QueuedPlayer>()
                .Property(e => e.PlayerId)
                .HasColumnType("char(36)");

            // modelBuilder.Entity<Session>().HasKey(s => s.SessionId);
            // modelBuilder.Entity<QueuedPlayer>().HasKey(q => q.Id);
        }
    }
}
