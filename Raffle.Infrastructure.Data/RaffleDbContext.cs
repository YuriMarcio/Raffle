using Microsoft.EntityFrameworkCore;
using Raffle.Domain.Entities.Raffles;
using Raffle.Domain.Entities.Tickets;
using Raffle.Domain.Entities.Payments;
using Raffle.Domain.Entities;

namespace Raffle.Infrastructure.Data
{
    public class RaffleDbContext : DbContext
    {
        public RaffleDbContext(DbContextOptions<RaffleDbContext> options) : base(options)
        {
        }

        // Definição de DbSet para a tabela "Tickets"
        public DbSet<Ticket> Tickets { get; set; }

        // Definição de DbSet para a tabela "Prizes"
        public DbSet<Prize> Prizes { get; set; }

        // Definição de DbSet para a tabela "Users"
        public DbSet<User> Users { get; set; }

        // Definição de DbSet para a tabela "Raffles"
        public DbSet<RaffleEntity> Raffles { get; set; } = null!;

        // Definição de DbSet para a tabela "Payments"
        public DbSet<Payment> Payments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento de User com Name (obrigatório e com limite de 100 caracteres)
            modelBuilder.Entity<User>()
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

            // Relacionamento entre Ticket e User
            modelBuilder.Entity<Ticket>()
               .HasOne(t => t.User) // Um Ticket pertence a um User
               .WithMany(c => c.Tickets) // Um User pode ter muitos Tickets
               .HasForeignKey(t => t.UserId) // A chave estrangeira é UserId em Ticket
               .OnDelete(DeleteBehavior.Cascade); // Se um User for excluído, todos os seus Tickets serão excluídos

            // Relacionamento entre RaffleClient, User e Raffle (Tabela de relacionamento muitos-para-muitos)
            modelBuilder.Entity<RaffleClient>()
                .HasKey(rc => new { rc.UserId, rc.RaffleId }); // Definindo chave composta

            modelBuilder.Entity<RaffleClient>()
                .HasOne(rc => rc.User)
                .WithMany(c => c.RaffleClients)
                .HasForeignKey(rc => rc.UserId);

            // Relacionamento entre Payment e Ticket (1 pagamento pode pagar vários tickets)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Payment) // Um Ticket pertence a um Payment
                .WithMany(p => p.Tickets) // Um Payment pode pagar vários Tickets
                .HasForeignKey(t => t.PaymentId)
                .OnDelete(DeleteBehavior.Restrict); // Restrição para evitar exclusão acidental

            // Relacionamento entre Payment e User (quem realiza o pagamento)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
