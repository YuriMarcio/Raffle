using Raffle.Domain.Entities.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(int id);
    }
}
