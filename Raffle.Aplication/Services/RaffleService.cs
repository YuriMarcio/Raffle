using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Raffle.Aplication.DTOs.RaffleDto;
using Raffle.Domain.Entities;
using Raffle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Raffle.Domain.Entities.Tickets;
using LeadSoft.Common.Library.Extensions;
using Raffle.Application.Interfaces;
using Raffle.Domain.Entities.Raffle;
using Raffle.Aplication.DTOs.Prize;



namespace Raffle.Infrastructure.Services
{
    public class RaffleService : IRaffleService
    {
        private readonly RaffleDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;
        public RaffleService(RaffleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateRaffleDtoResponse> CreateRaffleAsync(CreateRaffleDtoRequest dtoRequest)
        {
            // Criação da rifa
            var raffle = new RaffleEntity
            {
                Id = Guid.NewGuid().ToString(),
                Title = dtoRequest.Title,
                Description = dtoRequest.Description,
                Type = dtoRequest.Type,
                Images = dtoRequest.Images,
                ImageBanner = dtoRequest.ImageBanner,
                StartDate = dtoRequest.StartDate,
                EndDate = dtoRequest.EndDate,
                Terms = dtoRequest.Terms,
                TicketPrice = dtoRequest.TicketPrice,
                MaxTicketPerUser = dtoRequest.MaxTicketPerUser,
                MaxParticipants = dtoRequest.MaxParticipants,
                PaymentMethod = dtoRequest.PaymentMethod,
                IsEnable = dtoRequest.IsEnable,
                Status = dtoRequest.Status,
                CoverImageUrl = dtoRequest.CoverImageUrl,
                UniqueLink = GenerateUniqueLink()
            };

            // Adiciona a rifa ao contexto
            _context.Raffles.Add(raffle);
            await _context.SaveChangesAsync();

            // Cria os prêmios e os adiciona ao contexto
            var prizes = dtoRequest.Prizes.Select(p => new Prize
            {
                Id = Guid.NewGuid().ToString(),
                Title = p.Title,
                Description = p.Description,
                Value = p.Value,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,
                Type = p.Type,
                RaffleId = raffle.Id
            }).ToList();

            _context.Prizes.AddRange(prizes);
            await _context.SaveChangesAsync();

            // Gera os tickets
            var tickets = Ticket.GenerateTickets(raffle.Id, dtoRequest.TicketsSold);

            // Adiciona os tickets ao contexto
            _context.Tickets.AddRange(tickets);
            await _context.SaveChangesAsync();

            // Retorna o resultado da criação
            return new CreateRaffleDtoResponse
            {
                Id = raffle.Id.ToGuid(),
                Title = raffle.Title
            };
        }




        public async Task<IEnumerable<RaffleEntity>> GetAllRafflesAsync()
        {
            return await _context.Raffles.ToListAsync();
        }

        public async Task<RaffleDto> GetRaffleByIdAsync(string aID)
        {
            var raffle = await _context.Raffles
                .Include(r => r.Prizes)
                .Include(r => r.Tickets)
                .Include(r => r.RaffleClients)
                .FirstOrDefaultAsync(r => r.Id == aID);
                
            if (raffle == null)
                return null;

            // Usar AutoMapper para mapear todos os campos
            return _mapper.Map<RaffleDto>(raffle);
        }

        public async Task<RaffleDto> UpdateRaffleAsync(string aID, UpdateRaffleDtoRequest dtoRequest)
        {
            var raffle = await _context.Raffles
                .Include(r => r.Prizes)
                .Include(r => r.Tickets)
                .Include(r => r.RaffleClients)
                .FirstOrDefaultAsync(r => r.Id == aID);
                
            if (raffle == null)
                throw new KeyNotFoundException("Rifa não encontrada.");

            // Use AutoMapper for the update
            _mapper.Map(dtoRequest, raffle);
            
            // Handle prizes update if provided
            if (dtoRequest.Prizes != null && dtoRequest.Prizes.Any())
            {
                // Remove existing prizes
                _context.Prizes.RemoveRange(raffle.Prizes);
                
                // Add new prizes
                var newPrizes = dtoRequest.Prizes.Select(p => new Prize
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = p.Title,
                    Description = p.Description,
                    Value = p.Value,
                    ImageUrl = p.ImageUrl,
                    Quantity = p.Quantity,
                    Type = (Domain.Enums.PrizeType)p.Type,
                    RaffleId = raffle.Id
                }).ToList();
                
                _context.Prizes.AddRange(newPrizes);
            }
            
            // UpdatedAt will be updated automatically by Entity Framework if configured
            await _context.SaveChangesAsync();

            // Map entity to DTO using AutoMapper
            return _mapper.Map<RaffleDto>(raffle);
        }

        public async Task DeleteRaffleAsync(string aID)
        {
            var raffle = await _context.Raffles.FindAsync(aID);
            if (raffle == null)
                throw new KeyNotFoundException("Rifa não encontrada.");

            _context.Raffles.Remove(raffle);
            await _context.SaveChangesAsync();
        }

        private string GenerateUniqueLink()
        {
            return Guid.NewGuid().ToString("N")[..8];
        }

        public async Task<RaffleEntity> GetByUniqueLinkAsync(string uniqueLink)
        {
            return await _context.Raffles
                .Include(r => r.Tickets)
                .FirstOrDefaultAsync(r => r.UniqueLink == uniqueLink);
        }

        public async Task<List<int>> GetAvailableTicketsByUniqueLinkAsync(string uniqueLink)
        {
            var raffle = await _context.Raffles
                .Include(r => r.Tickets)
                .FirstOrDefaultAsync(r => r.UniqueLink == uniqueLink);

            if (raffle == null)
                throw new KeyNotFoundException("Rifa não encontrada");

            var soldTickets = raffle.Tickets.Where(t => t.UserId != null).Select(t => t.Value).ToList();
            var availableTickets = new List<int>();

            for (int i = 1; i <= 100; i++)
            {
                if (!soldTickets.Contains(i))
                {
                    availableTickets.Add(i);
                }
            }

            return availableTickets;
        }

        public async Task<object> PurchaseTicketsPublicAsync(string uniqueLink, List<int> ticketNumbers, object customerInfo)
        {
            var raffle = await GetByUniqueLinkAsync(uniqueLink);
            if (raffle == null)
                throw new KeyNotFoundException("Rifa não encontrada");

            var availableTickets = await GetAvailableTicketsByUniqueLinkAsync(uniqueLink);
            var invalidNumbers = ticketNumbers.Where(n => !availableTickets.Contains(n)).ToList();

            if (invalidNumbers.Any())
                throw new ArgumentException($"Números não disponíveis: {string.Join(", ", invalidNumbers)}");

            var newTickets = ticketNumbers.Select(number => new Ticket
            {
                Id = Guid.NewGuid().ToString(),
                Value = number,
                RaffleId = raffle.Id,
                UserId = null,
                PurchaseDate = DateTime.UtcNow,
                IsWinner = false
            }).ToList();

            _context.Tickets.AddRange(newTickets);
            await _context.SaveChangesAsync();

            return new { Message = "Números reservados com sucesso", TicketNumbers = ticketNumbers };
        }

        public async Task<object> GetTicketByUniqueLinkAsync(string uniqueLink, int ticketNumber)
        {
            var raffle = await GetByUniqueLinkAsync(uniqueLink);
            if (raffle == null)
                throw new KeyNotFoundException("Rifa não encontrada");

            var ticket = raffle.Tickets.FirstOrDefault(t => t.Value == ticketNumber);
            if (ticket == null)
                return null;

            return new
            {
                Number = ticket.Value,
                IsAvailable = ticket.UserId == null,
                PurchaseDate = ticket.PurchaseDate
            };
        }
    }
}
