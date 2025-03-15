namespace Raffle.Application.DTOs.Responses
{
    public class TicketResponse
    {
        public string Id { get; set; } = string.Empty;
        public int Value { get; set; }
        public string? UserId { get; set; }
        public string RaffleId { get; set; } = string.Empty;
        public bool IsWinner { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEnabled { get; set; }
    }
}
