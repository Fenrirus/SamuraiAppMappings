using Microsoft.EntityFrameworkCore;
using SamuraiAppMappings.Domain;

namespace SamuraiAppMappings.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Samurai> Samurais { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                 "Server = (localdb)\\mssqllocaldb; Database = SamuraiApp2Data; Trusted_Connection = True; ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.SamuraiId, s.BattleId });

            // base.OnModelCreating(modelBuilder);
        }
    }
}