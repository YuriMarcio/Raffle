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
        /// Identificador único do sorteio promocional
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Título do sorteio promocional
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Descrição detalhada do sorteio promocional
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Tipo do sorteio promocional (enum)
        /// </summary>
        public RaffleType Type { get; set; }

        /// <summary>
        /// Lista de URLs das imagens
        /// </summary>
        public IList<string> Images { get; set; } = new List<string>();

        /// <summary>
        /// URL do banner do sorteio promocional (opcional)
        /// </summary>
        public string ImageBanner { get; set; } = string.Empty;

        /// <summary>
        /// Data de início do sorteio promocional
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Data de término do sorteio promocional (opcional)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Termos e condições do sorteio promocional
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
        /// Status de habilitação do sorteio promocional
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// Status do sorteio promocional (enum)
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
        /// Link único para acesso público ao sorteio promocional
        /// </summary>
        public string UniqueLink { get; set; }

        /// <summary>
        /// Número total de tickets/números do sorteio promocional
        /// </summary>
        public int NumberOfTickets { get; set; } = 100;

        /// <summary>
        /// ID do tema personalizado do sorteio promocional
        /// </summary>
        public string ThemeId { get; set; }

        /// <summary>
        /// ID da empresa criadora do sorteio promocional
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Empresa criadora do sorteio promocional
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Indica se a entidade está ativa ou não
        /// </summary>
        public virtual bool IsEnabled { get; private set; }

        /// <summary>
        /// Data de criação do sorteio promocional
        /// </summary>
        public virtual DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Data de última atualização do sorteio promocional
        /// </summary>
        public virtual DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Relacionamento com os tickets do sorteio promocional
        /// </summary>
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        /// <summary>
        /// Lista de prêmios do sorteio promocional
        /// </summary>
        public ICollection<Prize> Prizes { get; set; } = new List<Prize>();

        /// <summary>
        /// Relacionamento com os participantes do sorteio promocional
        /// </summary>
        public ICollection<RaffleClient> RaffleClients { get; set; } = new List<RaffleClient>();

        // Campos de Conformidade Legal (Lei 5.768/71)

        /// <summary>
        /// Número da autorização SCPC para este sorteio promocional (obrigatório Lei 5.768/71)
        /// </summary>
        [MaxLength(50, ErrorMessage = "O número de autorização não pode exceder 50 caracteres.")]
        public string SCPCAuthorizationNumber { get; set; }

        /// <summary>
        /// Data de emissão da autorização SCPC para este sorteio promocional
        /// </summary>
        public DateTime SCPCAuthorizationDate { get; set; }

        /// <summary>
        /// Data de validade da autorização SCPC para este sorteio promocional
        /// </summary>
        public DateTime SCPCExpirationDate { get; set; }

        /// <summary>
        /// Regulamento completo do sorteio promocional (obrigatório)
        /// </summary>
        public string Regulation { get; set; }

        /// <summary>
        /// Finalidade da arrecadação (obrigatório para entidades sem fins lucrativos)
        /// </summary>
        [MaxLength(500, ErrorMessage = "A finalidade não pode exceder 500 caracteres.")]
        public string FundraisingPurpose { get; set; }

        /// <summary>
        /// Data do sorteio
        /// </summary>
        public DateTime DrawDate { get; set; }

        /// <summary>
        /// Local ou plataforma do sorteio
        /// </summary>
        [MaxLength(200, ErrorMessage = "O local do sorteio não pode exceder 200 caracteres.")]
        public string DrawLocation { get; set; }

        /// <summary>
        /// URL da transmissão ao vivo do sorteio
        /// </summary>
        [MaxLength(500, ErrorMessage = "A URL da transmissão não pode exceder 500 caracteres.")]
        public string DrawLiveStreamUrl { get; set; }

        /// <summary>
        /// Indica se o sorteio foi realizado
        /// </summary>
        public bool IsDrawn { get; set; }

        /// <summary>
        /// Data em que o sorteio foi realizado
        /// </summary>
        public DateTime? DrawnAt { get; set; }

        /// <summary>
        /// URL da ata do sorteio (PDF)
        /// </summary>
        [MaxLength(500, ErrorMessage = "A URL da ata não pode exceder 500 caracteres.")]
        public string DrawMinutesUrl { get; set; }

        /// <summary>
        /// Status de prestação de contas
        /// </summary>
        public string AccountabilityStatus { get; set; } = "PENDING"; // PENDING, SUBMITTED, APPROVED, REJECTED

        /// <summary>
        /// Data de submissão da prestação de contas
        /// </summary>
        public DateTime? AccountabilitySubmittedAt { get; set; }

        /// <summary>
        /// URL dos documentos de prestação de contas
        /// </summary>
        [MaxLength(500, ErrorMessage = "A URL dos documentos não pode exceder 500 caracteres.")]
        public string AccountabilityDocumentsUrl { get; set; }

        /// <summary>
        /// Observações sobre a prestação de contas
        /// </summary>
        public string AccountabilityNotes { get; set; }

        /// <summary>
        /// Avisos legais obrigatórios
        /// </summary>
        public string LegalWarnings { get; set; } = "Operação autorizada pela SPA/MF. Proibida a venda para menores de 18 anos.";
    }
}
