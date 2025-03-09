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
using Raffle.Domain.Entities.Raffle;



namespace Raffle.Infrastructure.Services
{
    public class RaffleService : IRaffleService
    {
        private readonly RaffleDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;
        public RaffleService(RaffleDbContext context)
        {
            _context = context;

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
            var raffle = await _context.Raffles.FindAsync(aID);
            if (raffle == null)
                return null;

            return new RaffleDto { Title = raffle.Title, Description = raffle.Description };
        }

        public async Task<RaffleDto> UpdateRaffleAsync(string aID, UpdateRaffleDtoRequest dtoRequest)
        {
            var raffle = await _context.Raffles.FindAsync(aID);
            if (raffle == null)
                throw new KeyNotFoundException("Rifa não encontrada.");


            _mapper.Map(dtoRequest, raffle);

            await _context.SaveChangesAsync();

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
    }
}
