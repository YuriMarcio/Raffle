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

        /// <summary>
        /// MÉTODO DESABILITADO - Lei 5.768/71: Apenas empresas (CNPJ) podem criar sorteios promocionais.
        /// Pessoas físicas (CPF) NÃO podem realizar sorteios promocionais.
        /// </summary>
        [Obsolete("Registro de CPF para sorteios promocionais é proibido pela Lei 5.768/71. Use apenas RegisterCNPJAsync.", true)]
        public async Task<string> RegisterCPFAsync(RegisterCPFRequest request)
        {
            // Verificar se o email já existe
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email já cadastrado.");
            }

            // Verificar se o CPF já existe
            var existingCPF = await _context.Users.FirstOrDefaultAsync(u => u.DocumentNumber == request.CPF);
            if (existingCPF != null)
            {
                throw new InvalidOperationException("CPF já cadastrado.");
            }

            // Criar novo usuário
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone,
                DocumentType = "CPF",
                DocumentNumber = request.CPF,
                DocumentVerificationStatus = "PENDING",
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                State = request.State,
                IsEmailVerified = false,
                IsPhoneVerified = false,
                IsAdmin = false,
                IsEnabled = true,
                CanCreateRaffles = false, // Só pode criar sorteios promocionais após verificação
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<string> RegisterCNPJAsync(RegisterCNPJRequest request)
        {
            // Verificar se o email já existe
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email já cadastrado.");
            }

            // Verificar se o CNPJ já existe
            var existingCNPJ = await _context.Users.FirstOrDefaultAsync(u => u.DocumentNumber == request.CNPJ);
            if (existingCNPJ != null)
            {
                throw new InvalidOperationException("CNPJ já cadastrado.");
            }

            // Criar novo usuário empresa
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.ResponsibleName,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone,
                DocumentType = "CNPJ",
                DocumentNumber = request.CNPJ,
                CompanyName = request.CompanyName,
                TradeName = request.TradeName,
                DocumentVerificationStatus = "PENDING",
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                State = request.State,
                IsEmailVerified = false,
                IsPhoneVerified = false,
                IsAdmin = false,
                IsEnabled = true,
                CanCreateRaffles = false, // Só pode criar sorteios promocionais após verificação
                RaffleCreationLimit = 10, // Limite inicial para empresas
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<object> GetDocumentVerificationStatus(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            return new
            {
                userId = user.Id,
                documentType = user.DocumentType,
                documentNumber = user.DocumentNumber,
                verificationStatus = user.DocumentVerificationStatus,
                verifiedAt = user.DocumentVerifiedAt,
                notes = user.DocumentVerificationNotes,
                canCreateRaffles = user.CanCreateRaffles
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("DocumentType", user.DocumentType ?? ""),
                new Claim("CanCreateRaffles", user.CanCreateRaffles.ToString())
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