using LeadSoft.Common.GlobalDomain.Entities;
using Raffle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Raffle.Domain.Entities
{
    public class Prize : CollectionsBase
    {
        // Propriedades herdadas de CollectionsBase
        /// <summary>
        /// Identificador único do prêmio no banco de dados.
        /// </summary>
        [Key]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o prêmio está habilitado ou desabilitado no sistema.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Data e hora em que o prêmio foi criado no sistema.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data e hora da última atualização do prêmio no sistema (opcional).
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Propriedades específicas da entidade Prize com validações
        /// <summary>
        /// Título do prêmio.
        /// </summary>
        [Required(ErrorMessage = "O título do prêmio é obrigatório.")]
        [MaxLength(200, ErrorMessage = "O título não pode exceder 200 caracteres.")]
        public string Title { get; set; }

        /// <summary>
        /// Descrição do prêmio (opcional).
        /// </summary>
        [MaxLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public string? Description { get; set; }

        /// <summary>
        /// Valor do prêmio (opcional).
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do prêmio deve ser maior que zero.")]
        public decimal? Value { get; set; }

        /// <summary>
        /// URL da imagem do prêmio (opcional).
        /// </summary>
        [Url(ErrorMessage = "A URL da imagem não é válida.")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Quantidade de prêmios disponíveis.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade de prêmios deve ser maior que zero.")]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Tipo do prêmio.
        /// </summary>
        [Required(ErrorMessage = "O tipo do prêmio é obrigatório.")]
        public PrizeType Type { get; set; }

        /// <summary>
        /// Identificador da rifa associada ao prêmio (opcional).
        /// </summary>
        public string? RaffleId { get; set; }
    }
}
