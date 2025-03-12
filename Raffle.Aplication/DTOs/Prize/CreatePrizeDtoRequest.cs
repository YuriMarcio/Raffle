using System.ComponentModel.DataAnnotations;
using Raffle.Domain.Enums;

namespace Raffle.Aplication.DTOs.Prize
{
    public class CreatePrizeDtoRequest
    {
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
    }
}
