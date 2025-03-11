using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Aplication.DTOs.Prize
{
    public class PrizeDtoResponse
    {
        private Domain.Entities.Prize prize;

        public PrizeDtoResponse(Domain.Entities.Prize prize)
        {
            this.prize = prize;
        }

        public string Id { get; set; } 
        public string Title { get; set; }
        public string RaffleId { get; set; }
    }
}
