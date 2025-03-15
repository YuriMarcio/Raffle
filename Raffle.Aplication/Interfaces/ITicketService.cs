using Raffle.Application.DTOs.Requests;
using Raffle.Application.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Application.Services.Interfaces
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
