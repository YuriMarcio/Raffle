using Raffle.Domain.Enums;
using System;

namespace Raffle.Aplication.DTOs.Prize
{
    public class PrizeDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal? Value { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; } = 1;
        public PrizeType Type { get; set; }
        public int? RaffleId { get; set; }
    }
}
