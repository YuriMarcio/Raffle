using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Aplication.DTOs.RaffleDto
{
    public class CreateRaffleDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
