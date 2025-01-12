using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Raffle.Domain.Entities;

namespace Raffle.Infrastructure.Data
{
    public class RaffleDbContext : DbContext
    {
        public RaffleDbContext(DbContextOptions<RaffleDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RaffleEntity> Raffles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Prize> Prizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Client)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.ClientId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Raffle)
                .WithMany(r => r.Tickets)
                .HasForeignKey(t => t.RaffleId);

            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<RaffleEntity>()
                .Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Prize>()
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}

