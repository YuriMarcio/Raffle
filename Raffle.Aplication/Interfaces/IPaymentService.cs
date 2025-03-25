using Raffle.Domain.Entities.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Application.Services
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(string userId, decimal amount);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<List<Payment>> GetPaymentsByUserAsync(string userId);
        Task<Payment> UpdatePaymentStatusAsync(int id, string status);
    }
}
