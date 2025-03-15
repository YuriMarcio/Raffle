using Microsoft.EntityFrameworkCore;
using Raffle.Domain.Entities;
using Raffle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RaffleDbContext _context;

        public UserRepository(RaffleDbContext context)
        {
            _context = context;
        }

        // Obter um usuário pelo ID
        public async Task<User> GetByIdAsync(string userId)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Id == userId);
        }

        // Obter todos os usuários
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Adicionar um novo usuário
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // Atualizar um usuário existente
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // Deletar um usuário pelo ID
        public async Task DeleteAsync(string userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
