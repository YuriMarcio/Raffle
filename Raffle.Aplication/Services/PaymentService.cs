using Microsoft.EntityFrameworkCore;
using Raffle.Domain.Entities.Payments;
using Raffle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly RaffleDbContext _context;

        public PaymentService(RaffleDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync(string userId, decimal amount)
        {
            var payment = new Payment
            {
                UserId = userId,
                Amount = amount,
                Status = "Pendente",
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<List<Payment>> GetPaymentsByUserAsync(string userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Payment> UpdatePaymentStatusAsync(int id, string status)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return null;

            payment.Status = status;
            await _context.SaveChangesAsync();

            return payment;
        }
    }
}
