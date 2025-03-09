using LeadSoft.Common.GlobalDomain.Entities;
using Raffle.Domain.Entities.Raffle;
using System;
using System.ComponentModel.DataAnnotations;

namespace Raffle.Domain.Entities.Tickets
{
    public partial class Ticket
    {
        public string Id { get; set; } = string.Empty; // Id da entidade

        public bool IsEnabled { get; set; } // Se a entidade está habilitada ou desabilitada

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Data de criação

        public DateTime? UpdatedAt { get; set; } // Data de última atualização (opcional)

        public int Value { get; set; } // Valor do ticket
        public string? ClientId { get; set; } // ID do cliente associado
        public string RaffleId { get; set; } // ID da rifa associada
        public bool IsWinner { get; set; } // Se o ticket é vencedor
        public DateTime? PurchaseDate { get; set; } // Data da compra

        public Client Client { get; set; } // Cliente associado
        public RaffleEntity Raffle { get; set; } // Rifa associada
    }
}
