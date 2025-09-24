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

            // Relacionamento entre Ticket e Client
            modelBuilder.Entity<Ticket>()
               .HasOne(t => t.User) // Um Ticket pertence a um Client
               .WithMany(c => c.Tickets) // Um Client pode ter muitos Tickets
               .HasForeignKey(t => t.UserId) // A chave estrangeira é ClientId em Ticket
               .OnDelete(DeleteBehavior.Cascade); // Se um Client for excluído, todos os seus Tickets serão excluídos

            // Relacionamento entre RaffleClient, Client e Raffle (Tabela de relacionamento muitos-para-muitos)
            modelBuilder.Entity<RaffleClient>()
                .HasKey(rc => new { rc.UserId, rc.RaffleId }); // Definindo chave composta

            modelBuilder.Entity<RaffleClient>()
                .HasOne(rc => rc.User)
                .WithMany(c => c.RaffleClients)
                .HasForeignKey(rc => rc.UserId);

            // Configure RaffleEntity explicit mapping
            modelBuilder.Entity<RaffleEntity>(entity =>
            {
                entity.ToTable("Raffles");
                entity.HasKey(e => e.Id);

                // Map properties explicitly to avoid convention conflicts
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Guid).HasColumnName("Guid");
                entity.Property(e => e.Title).HasColumnName("Title");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.Type).HasColumnName("Type").HasConversion<string>();
                entity.Property(e => e.Images).HasColumnName("Images");
                entity.Property(e => e.ImageBanner).HasColumnName("ImageBanner").IsRequired(false);
                entity.Property(e => e.StartDate).HasColumnName("StartDate");
                entity.Property(e => e.EndDate).HasColumnName("EndDate");
                entity.Property(e => e.Terms).HasColumnName("Terms");
                entity.Property(e => e.TicketPrice).HasColumnName("TicketPrice");
                entity.Property(e => e.MaxTicketPerUser).HasColumnName("MaxTicketPerUser");
                entity.Property(e => e.MaxParticipants).HasColumnName("MaxParticipants");
                entity.Property(e => e.PaymentMethod).HasColumnName("PaymentMethod").HasConversion<string>();
                entity.Property(e => e.IsEnable).HasColumnName("IsEnable");
                entity.Property(e => e.Status).HasColumnName("Status").HasConversion<string>();
                entity.Property(e => e.CoverImageUrl).HasColumnName("CoverImageUrl");
                entity.Property(e => e.TicketsSold).HasColumnName("TicketsSold");
                entity.Property(e => e.UniqueLink).HasColumnName("UniqueLink");
                entity.Property(e => e.NumberOfTickets).HasColumnName("NumberOfTickets");
                entity.Property(e => e.ThemeId).HasColumnName("ThemeId");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.IsEnabled).HasColumnName("IsEnabled");
                entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
                entity.Property(e => e.SCPCAuthorizationNumber).HasColumnName("SCPCAuthorizationNumber");
                entity.Property(e => e.SCPCAuthorizationDate).HasColumnName("SCPCAuthorizationDate");
                entity.Property(e => e.SCPCExpirationDate).HasColumnName("SCPCExpirationDate");
                entity.Property(e => e.Regulation).HasColumnName("Regulation");
                entity.Property(e => e.FundraisingPurpose).HasColumnName("FundraisingPurpose");
                entity.Property(e => e.DrawDate).HasColumnName("DrawDate");
                entity.Property(e => e.DrawLocation).HasColumnName("DrawLocation");
                entity.Property(e => e.DrawLiveStreamUrl).HasColumnName("DrawLiveStreamUrl");
                entity.Property(e => e.IsDrawn).HasColumnName("IsDrawn");
                entity.Property(e => e.DrawnAt).HasColumnName("DrawnAt");
                entity.Property(e => e.DrawMinutesUrl).HasColumnName("DrawMinutesUrl");
                entity.Property(e => e.AccountabilityStatus).HasColumnName("AccountabilityStatus");
                entity.Property(e => e.AccountabilitySubmittedAt).HasColumnName("AccountabilitySubmittedAt");
                entity.Property(e => e.AccountabilityDocumentsUrl).HasColumnName("AccountabilityDocumentsUrl");
                entity.Property(e => e.AccountabilityNotes).HasColumnName("AccountabilityNotes");
                entity.Property(e => e.LegalWarnings).HasColumnName("LegalWarnings");
            });
        }
    }
}
