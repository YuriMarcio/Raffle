using Raffle.Aplication.DTOs.Prize;
using Raffle.Domain.Entities;
using Raffle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Aplication.DTOs.RaffleDto
{
    public class CreateRaffleDtoRequest
    {
        public string Title { get; set; } // Título da rifa
        public string Description { get; set; } // Descrição detalhada
        public RaffleType Type { get; set; } // Tipo de rifa (enum)
        public IList<string>? Images { get; set; }
        public string? ImageBanner { get; set; } = null;

        public DateTime StartDate { get; set; } // Data de início
        public DateTime? EndDate { get; set; } // Data de término

        public string? Terms { get; set; } // Termos e condições

        //com a quatiadade de ticketes eu gero os tickets nescessarios 
        public int TicketsSold { get; set; } // Total de bilhetes 
        public decimal TicketPrice { get; set; } // Preço de cada bilhete
        public int MaxTicketPerUser { get; set; } // Limite de bilhetes por usuário
        public int MaxParticipants { get; set; } // Número máximo de participantes

        public PaymentMethod PaymentMethod { get; set; } // Método de pagamento aceito
        public bool IsEnable { get; set; } // Status de habilitação da rifa

        // Propriedades adicionais sugeridas
        public RaffleStatus Status { get; set; } // Status da rifa (enum)
        public string? CoverImageUrl { get; set; } // URL da imagem de capa


        // Lista de prêmios
        public ICollection<PrizeDto> Prizes { get; set; } = new List<PrizeDto>();

    }
}
