using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Raffle.Aplication.DTOs.AutenticationDto;
using Raffle.Domain.Entities;
using Raffle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Services
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

        public async Task<string> RegisterAsync(ResgisterDtoRequest aDtoRequest)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == aDtoRequest.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Usuário já existe.");
            }

            var user = new User
            {
                Name = aDtoRequest.Name,
                Email = aDtoRequest.Email,
                IsAdmin = aDtoRequest.IsAdmin,
                Password = BCrypt.Net.BCrypt.HashPassword(aDtoRequest.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Usuário registrado com sucesso.";
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

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

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