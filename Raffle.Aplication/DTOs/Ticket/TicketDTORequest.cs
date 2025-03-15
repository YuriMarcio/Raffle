namespace Raffle.Application.DTOs.Requests
{
    public class TicketDTORequest
    {
        public int Value { get; set; }
        public string? UserId { get; set; }
        public string RaffleId { get; set; } = string.Empty;
        public bool IsWinner { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
}
