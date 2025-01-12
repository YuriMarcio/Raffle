using Raffle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Raffle.Domain.Entities
{
    public class RaffleEntity
    {
        public int Id { get; set; } // Identificador único
        public string Title { get; set; } // Título da rifa
        public string Description { get; set; } // Descrição detalhada
        public RaffleType Type { get; set; } // Tipo de rifa (enum)
        public IList<string> images { get; set; }
        public string ImageBanner { get; set; } = null;

        public DateTime StartDate { get; set; } // Data de início
        public DateTime EndDate { get; set; } // Data de término

        public string Terms { get; set; } // Termos e condições

        public decimal TicketPrice { get; set; } // Preço de cada bilhete
        public int MaxTicketPerUser { get; set; } // Limite de bilhetes por usuário
        public int MaxParticipants { get; set; } // Número máximo de participantes

        public PaymentMethod PaymentMethod { get; set; } // Método de pagamento aceito
        public bool IsEnable { get; set; } // Status de habilitação da rifa

        // Propriedades adicionais sugeridas
        public RaffleStatus Status { get; set; } // Status da rifa (enum)
        public string CoverImageUrl { get; set; } // URL da imagem de capa
        public int TicketsSold { get; set; } // Total de bilhetes 

        // Lista de prêmios
        public ICollection<Prize> Prizes { get; set; } = new List<Prize>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>(); 
    }
}
