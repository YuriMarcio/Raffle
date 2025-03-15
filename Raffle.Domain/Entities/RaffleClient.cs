using Raffle.Domain.Entities.Raffle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Domain.Entities
{
    public class RaffleClient
    {
        public string UserId { get; set; }  // Chave estrangeira para Client
        public User User { get; set; }  // Propriedade de navegação para Client

        public string RaffleId { get; set; }  // Chave estrangeira para Raffle
        public RaffleEntity Raffle { get; set; }  // Propriedade de navegação para Raffle

        public DateTime JoinDate { get; set; } // (Exemplo) - Outras informações sobre a relação
    }
}
