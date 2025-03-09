using LeadSoft.Common.GlobalDomain.Entities;
using Raffle.Domain.Entities.Tickets;
using Raffle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Raffle.Domain.Entities.Raffle
{
    public class RaffleEntity : CollectionsBase
    {
        /// <summary>
        /// Identificador único da rifa
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Título da rifa
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Descrição detalhada da rifa
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Tipo da rifa (enum)
        /// </summary>
        public RaffleType Type { get; set; }

        /// <summary>
        /// Lista de URLs das imagens
        /// </summary>
        public IList<string> Images { get; set; } = new List<string>();

        /// <summary>
        /// URL do banner da rifa (opcional)
        /// </summary>
        public string ImageBanner { get; set; } = string.Empty;

        /// <summary>
        /// Data de início da rifa
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Data de término da rifa (opcional)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Termos e condições da rifa
        /// </summary>
        public string Terms { get; set; }

        /// <summary>
        /// Preço de cada bilhete
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço do bilhete deve ser maior que zero.")]
        public decimal TicketPrice { get; set; }

        /// <summary>
        /// Quantidade máxima de bilhetes por usuário
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O número máximo de bilhetes por usuário deve ser maior que zero.")]
        public int MaxTicketPerUser { get; set; }

        /// <summary>
        /// Número máximo de participantes
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O número máximo de participantes deve ser maior que zero.")]
        public int MaxParticipants { get; set; }

        /// <summary>
        /// Método de pagamento aceito
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Status de habilitação da rifa
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// Status da rifa (enum)
        /// </summary>
        public RaffleStatus Status { get; set; }

        /// <summary>
        /// URL da imagem de capa
        /// </summary>
        public string CoverImageUrl { get; set; }

        /// <summary>
        /// Total de bilhetes vendidos
        /// </summary>
        public int TicketsSold { get; set; }

        /// <summary>
        /// Indica se a entidade está ativa ou não
        /// </summary>
        public virtual bool IsEnabled { get; private set; }

        /// <summary>
        /// Data de criação da rifa
        /// </summary>
        public virtual DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Data de última atualização da rifa
        /// </summary>
        public virtual DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Relacionamento com os tickets da rifa
        /// </summary>
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        /// <summary>
        /// Lista de prêmios da rifa
        /// </summary>
        public ICollection<Prize> Prizes { get; set; } = new List<Prize>();

        /// <summary>
        /// Relacionamento com os clientes da rifa
        /// </summary>
        public ICollection<RaffleClient> RaffleClients { get; set; } = new List<RaffleClient>();
    }
}
