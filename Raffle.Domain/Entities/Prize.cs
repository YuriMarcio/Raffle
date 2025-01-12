using Raffle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Domain.Entities
{
    public class Prize
    {
        public int Id { get; set; } 
        public string Title { get; set; } 
        public string Description { get; set; } 
        public decimal? Value { get; set; } 
        public string ImageUrl { get; set; } 
        public int Quantity { get; set; } = 1;

        public PrizeType Type { get; set; }

        public int RaffleId { get; set; } 
        public RaffleEntity Raffle { get; set; } 
    }
}
