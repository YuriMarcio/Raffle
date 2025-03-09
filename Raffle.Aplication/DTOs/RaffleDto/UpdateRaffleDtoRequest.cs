using Raffle.Aplication.DTOs.Prize;
using System;
using System.Collections.Generic;

namespace Raffle.Aplication.DTOs.RaffleDto
{
    public class UpdateRaffleDtoRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
        public List<string> Images { get; set; } = new();
        public string ImageBanner { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Terms { get; set; } = string.Empty;
        public int TicketsSold { get; set; }
        public decimal TicketPrice { get; set; }
        public int MaxTicketPerUser { get; set; }
        public int MaxParticipants { get; set; }
        public int PaymentMethod { get; set; }
        public bool IsEnable { get; set; }
        public int Status { get; set; }
        public string CoverImageUrl { get; set; } = string.Empty;
        public List<UpdatePrizeDtoRequest> Prizes { get; set; } = new();
    }
}
