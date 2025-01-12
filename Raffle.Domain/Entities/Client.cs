using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Domain.Entities
{
    public class Client
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } = null;
        public bool IsEmailverited { get; set; }
        public string Password { get; set; }
        public string phone { get; set; }
        public bool IsPhoneverited { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}
