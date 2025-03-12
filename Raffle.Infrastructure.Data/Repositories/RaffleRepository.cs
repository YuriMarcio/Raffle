using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Raffle.Domain.Entities;
using Raffle.Domain.Entities.Raffle;
using Raffle.Infrastructure.Data;

namespace Raffle.Infrastructure.Repositories
{
    public class RaffleRepository : IRaffleRepository
    {
        private readonly RaffleDbContext _context;

        public RaffleRepository(RaffleDbContext context)
        {
            _context = context;
        }

        public async Task<RaffleEntity?> GetByIdAsync(string raffleId)
        {
            return await _context.Raffles.FindAsync(raffleId);
        }

        public async Task AddAsync(RaffleEntity raffle)
        {
            await _context.Raffles.AddAsync(raffle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RaffleEntity raffle)
        {
            _context.Raffles.Update(raffle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string raffleId)
        {
            var raffle = await GetByIdAsync(raffleId);
            if (raffle != null)
            {
                _context.Raffles.Remove(raffle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
