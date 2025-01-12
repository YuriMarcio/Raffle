using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Domain.Entities
{
    public class Ticket
    { 
        public int Id { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
        public int RaffleId { get; set; } 
        public RaffleEntity Raffle { get; set; } 
        public string TicketNumber { get; set; }
        public bool IsWinner { get; set; } 
        public DateTime PurchaseDate { get; set; } 
    }
}
