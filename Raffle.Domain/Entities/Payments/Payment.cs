using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Raffle.Domain.Entities.Tickets;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace Raffle.Domain.Entities.Payments
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Método de pagamento aceito
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pendente"; // Pode ser 'Pendente', 'Aprovado', 'Recusado'

        // Relacionamento com os Tickets
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>(); // Aqui está o relacionamento com os tickets


        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }


    }
}
