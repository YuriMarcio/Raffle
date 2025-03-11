using System.Threading.Tasks;
using Raffle.Domain.Entities;

namespace Raffle.Infrastructure.Repositories
{
    public interface IPrizeRepository
    {
        Task<Prize?> GetByIdAsync(string prizeId);
        Task AddAsync(Prize prize);
        Task UpdateAsync(Prize prize);
        Task DeleteAsync(string prizeId);
    }
}
