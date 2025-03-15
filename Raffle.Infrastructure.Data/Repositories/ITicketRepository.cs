using Raffle.Domain.Entities.Tickets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Application.Repositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(string id);
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket?> UpdateAsync(Ticket ticket);
        Task<bool> DeleteAsync(string id);
    }
}
