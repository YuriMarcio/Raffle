using Raffle.Application.DTOs.Requests;
using Raffle.Application.DTOs.Responses;

namespace Raffle.Aplication.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketResponse>> GetAllTicketsAsync();
        Task<TicketResponse?> GetTicketByIdAsync(string id);
        Task<TicketResponse> CreateTicketAsync(TicketDTORequest request);
        Task<TicketResponse?> UpdateTicketAsync(string id, TicketDTORequest request);
        Task<bool> DeleteTicketAsync(string id);
    }
}
