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

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("char(36)");
                entity.Property(e => e.LatencyLevel)
                      .IsRequired()
                      .HasDefaultValue(1)
                      .HasAnnotation("SqlServer:Check", "LatencyLevel >= 1");

                entity.Property(e => e.JoinedCount)
                      .HasDefaultValue(0)
                      .HasAnnotation("SqlServer:Check", "JoinedCount <= 10");
/*
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
*/
                entity.Property(e => e.StartsAt)
                      .IsRequired();

                entity.Property(e => e.EndsAt)
                      .IsRequired();
            });

            modelBuilder.Entity<QueuedPlayer>()
                .Property(e => e.Id)
                .HasColumnType("char(36)");

            modelBuilder.Entity<QueuedPlayer>()
                .Property(e => e.PlayerId)
                .HasColumnType("char(36)");
        }
    }
}
