using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Raffle.Domain.Entities;
using Raffle.Infrastructure.Data;

namespace Raffle.Infrastructure.Repositories
{
    public class PrizeRepository : IPrizeRepository
    {
        private readonly RaffleDbContext _context;
        private readonly IPrizeRepository _prizeRepository;

        public PrizeRepository(RaffleDbContext context)
        {
            _context = context;
        }

        public async Task<Prize?> GetByIdAsync(string prizeId)
        {
            return await _context.Prizes.FindAsync(prizeId);
        }

        public async Task AddAsync(Prize prize)
        {
            await _context.Prizes.AddAsync(prize);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prize prize)
        {
            _context.Prizes.Update(prize);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string prizeId)
        {
            var prize = await GetByIdAsync(prizeId);
            if (prize != null)
            {
                _context.Prizes.Remove(prize);
                await _context.SaveChangesAsync();
            }
        }
    }
}
