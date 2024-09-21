using Microsoft.EntityFrameworkCore;

namespace Rovio.MatchMaking.Repositories.Data
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
        }
    }
}
