using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Raffle.Aplication.DTOs.AutenticationDto;
using Raffle.Application.Interfaces;
using Raffle.Domain.Entities;
using Raffle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly RaffleDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(RaffleDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            return GenerateJwtToken(user);
        }

        public async Task<string> RegisterAsync(string name, string email, string password, string phone)
        {
            // Verificar se o email já existe
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email já cadastrado.");
            }

            // Criar novo usuário
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Phone = phone,
                IsEmailVerified = false,
                IsPhoneVerified = false,
                IsAdmin = false,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };
            
            // Adicionar claim de role se for admin
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}