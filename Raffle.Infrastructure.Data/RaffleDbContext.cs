using Microsoft.EntityFrameworkCore;
using Raffle.Domain.Entities.Raffle;
using Raffle.Domain.Entities.Tickets;
using Raffle.Domain.Entities;

namespace Raffle.Infrastructure.Data
{
    public class RaffleDbContext : DbContext
    {
        public RaffleDbContext(DbContextOptions<RaffleDbContext> options) : base(options)
        {
        }

        // Definição de DbSet para a tabela "Clients"
        public DbSet<Client> Clients { get; set; }

        // Definição de DbSet para a tabela "Tickets"
        public DbSet<Ticket> Tickets { get; set; }

        // Definição de DbSet para a tabela "Prizes"
        public DbSet<Prize> Prizes { get; set; }

        // Definição de DbSet para a tabela "Users"
        public DbSet<User> Users { get; set; }

        // Definição de DbSet para a tabela "Raffles"
        public DbSet<RaffleEntity> Raffles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento de Client com Name (obrigatório e com limite de 100 caracteres)
            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Relacionamento entre Ticket e Raffle
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Raffle) // Um Ticket pertence a um Raffle
                .WithMany(r => r.Tickets) // Um Raffle pode ter muitos Tickets
                .HasForeignKey(t => t.RaffleId); // A chave estrangeira é RaffleId em Ticket

            // Relacionamento entre Prize e Description (obrigatório e com limite de 200 caracteres)
            modelBuilder.Entity<Prize>()
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(200);

            // Relacionamento entre Ticket e Client
            modelBuilder.Entity<Ticket>()
               .HasOne(t => t.Client) // Um Ticket pertence a um Client
               .WithMany(c => c.Tickets) // Um Client pode ter muitos Tickets
               .HasForeignKey(t => t.ClientId) // A chave estrangeira é ClientId em Ticket
               .OnDelete(DeleteBehavior.Cascade); // Se um Client for excluído, todos os seus Tickets serão excluídos

            // Relacionamento entre RaffleClient, Client e Raffle (Tabela de relacionamento muitos-para-muitos)
            modelBuilder.Entity<RaffleClient>()
                .HasKey(rc => new { rc.ClientId, rc.RaffleId }); // Definindo a chave composta (ClientId, RaffleId)

            modelBuilder.Entity<RaffleClient>()
                .HasOne(rc => rc.Client) // Um RaffleClient pertence a um Client
                .WithMany(c => c.RaffleClients) // Um Client pode ter muitos RaffleClients
                .HasForeignKey(rc => rc.ClientId); // A chave estrangeira é ClientId em RaffleClient

            modelBuilder.Entity<RaffleClient>()
                .HasOne(rc => rc.Raffle) // Um RaffleClient pertence a um Raffle
                .WithMany(r => r.RaffleClients) // Um Raffle pode ter muitos RaffleClients
                .HasForeignKey(rc => rc.RaffleId); // A chave estrangeira é RaffleId em RaffleClient
        }
    }
}
