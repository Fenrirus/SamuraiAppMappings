using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiAppMappings.Domain;
using System;
using System.Linq;

namespace SamuraiAppMappings.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Samurai> Samurais { get; set; }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var time = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries().Where(e => (e.State == EntityState.Modified || e.State == EntityState.Added) && !e.Metadata.IsOwned()))
            {
                entry.Property("LastModified").CurrentValue = time;

                if (entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = time;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                 "Server = (localdb)\\mssqllocaldb; Database = SamuraiApp2Data; Trusted_Connection = True; ")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.SamuraiId, s.BattleId });

            //modelBuilder.Entity<Samurai>()
            //    .HasOne(s => s.SecretIdentity)
            //    .WithOne(i => i.Samurai).HasForeignKey<SecretIdentity>("SamuraiId");

            //modelBuilder.Entity<Samurai>().Property<DateTime>("Created");
            //modelBuilder.Entity<Samurai>().Property<DateTime>("LastModified");

            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).ToTable("BetterNames");

            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(b => b.GivenName).HasColumnName("GivenName");
            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(b => b.SurName).HasColumnName("SurName");

            //pamiętać, żeby było za ownsone
            foreach (var entities in modelBuilder.Model.GetEntityTypes().Where(t => !t.IsOwned()))
            {
                modelBuilder.Entity(entities.Name).Property<DateTime>("Created");
                modelBuilder.Entity(entities.Name).Property<DateTime>("LastModified");
            }
        }
    }
}