using System.Threading.Tasks;
using Raffle.Domain.Entities;
using Raffle.Domain.Entities.Raffle;

namespace Raffle.Infrastructure.Repositories
{
    public interface IRaffleRepository
    {
        Task<RaffleEntity?> GetByIdAsync(string raffleId);
        Task AddAsync(RaffleEntity raffle);
        Task UpdateAsync(RaffleEntity raffle);
        Task DeleteAsync(string raffleId);
    }
}
