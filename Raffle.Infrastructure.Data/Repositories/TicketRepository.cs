using Raffle.Application.Repositories;
using Raffle.Domain.Entities.Tickets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly List<Ticket> _tickets = new();

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await Task.FromResult(_tickets);
        }

        public async Task<Ticket?> GetByIdAsync(string id)
        {
            return await Task.FromResult(_tickets.FirstOrDefault(t => t.Id == id));
        }

        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            ticket.Id = Guid.NewGuid().ToString();
            _tickets.Add(ticket);
            return await Task.FromResult(ticket);
        }

        public async Task<Ticket?> UpdateAsync(Ticket ticket)
        {
            var existing = _tickets.FirstOrDefault(t => t.Id == ticket.Id);
            if (existing == null) return null;

            existing.Value = ticket.Value;
            existing.UserId = ticket.UserId;
            existing.RaffleId = ticket.RaffleId;
            existing.IsWinner = ticket.IsWinner;
            existing.PurchaseDate = ticket.PurchaseDate;
            existing.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(existing);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null) return false;

            _tickets.Remove(ticket);
            return await Task.FromResult(true);
        }
    }
}
