using Raffle.Aplication.DTOs.AutenticationDto;
using Raffle.Aplication.DTOs.RaffleDto;
using Raffle.Domain.Entities.Raffle;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Application.Interfaces
{
    public interface IRaffleService
    {
        Task<CreateRaffleDtoResponse> CreateRaffleAsync(CreateRaffleDtoRequest dtoRequest);
        Task DeleteRaffleAsync(string aId);
        Task<IEnumerable<RaffleEntity>> GetAllRafflesAsync();
        Task<RaffleDto> GetRaffleByIdAsync(string aID);
        Task<RaffleDto> UpdateRaffleAsync(string aID, UpdateRaffleDtoRequest dtoRequest);
        Task<RaffleEntity> GetByUniqueLinkAsync(string uniqueLink);
        Task<List<int>> GetAvailableTicketsByUniqueLinkAsync(string uniqueLink);
        Task<object> PurchaseTicketsPublicAsync(string uniqueLink, List<int> ticketNumbers, object customerInfo);
        Task<object> GetTicketByUniqueLinkAsync(string uniqueLink, int ticketNumber);
    }
}
