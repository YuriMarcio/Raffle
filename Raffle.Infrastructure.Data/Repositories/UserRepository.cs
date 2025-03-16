

using Amazon.CostExplorer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using Raffle.Domain.Entities;
using Raffle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            var user = await _context.Users.FindAsync(userId)
                ?? throw new InvalidOperationException("Usuário Não Exite");

            return user;
        }

        // Obter todos os usuários
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Adicionar um novo usuário
        public async Task<string> AddAsync(User aUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == aUser.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Usuário já existe.");
            }

            var user = new User
            {
                Name = aUser.Name,
                Email = aUser.Email,
                IsAdmin = aUser.IsAdmin,
                Password = BCrypt.Net.BCrypt.HashPassword(aUser.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return "Usuário registrado com sucesso.";

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
