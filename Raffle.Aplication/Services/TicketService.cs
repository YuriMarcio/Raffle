using Raffle.Application.DTOs.Requests;
using Raffle.Application.DTOs.Responses;
using Raffle.Application.Repositories;
using Raffle.Application.Services.Interfaces;
using Raffle.Domain.Entities.Tickets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raffle.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        // Construtor que injeta a dependência do repositório de Tickets
        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        // Retorna todos os tickets
        public async Task<IEnumerable<TicketResponse>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            return tickets.Select(MapToResponse);
        }

        // Retorna um ticket pelo ID
        public async Task<TicketResponse?> GetTicketByIdAsync(string id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            return ticket == null ? null : MapToResponse(ticket);
        }

        // Cria um novo ticket
        public async Task<TicketResponse> CreateTicketAsync(TicketDTORequest request)
        {
            var ticket = new Ticket
            {
                Value = request.Value,
                UserId = request.UserId,
                RaffleId = request.RaffleId,
                IsWinner = request.IsWinner,
                PurchaseDate = request.PurchaseDate,
                CreatedAt = DateTime.UtcNow,
                IsEnabled = true
            };

            var createdTicket = await _ticketRepository.CreateAsync(ticket);
            return MapToResponse(createdTicket);
        }

        // Atualiza um ticket existente
        public async Task<TicketResponse?> UpdateTicketAsync(string id, TicketDTORequest request)
        {
            var existingTicket = await _ticketRepository.GetByIdAsync(id);
            if (existingTicket == null) return null;

            existingTicket.Value = request.Value;
            existingTicket.UserId = request.UserId;
            existingTicket.RaffleId = request.RaffleId;
            existingTicket.IsWinner = request.IsWinner;
            existingTicket.PurchaseDate = request.PurchaseDate;
            existingTicket.UpdatedAt = DateTime.UtcNow;

            var updatedTicket = await _ticketRepository.UpdateAsync(existingTicket);
            return updatedTicket == null ? null : MapToResponse(updatedTicket);
        }

        // Exclui um ticket
        public async Task<bool> DeleteTicketAsync(string id)
        {
            return await _ticketRepository.DeleteAsync(id);
        }

        // Método auxiliar para mapear um Ticket para TicketResponse
        private static TicketResponse MapToResponse(Ticket ticket)
        {
            return new TicketResponse
            {
                Id = ticket.Id,
                Value = ticket.Value,
                UserId = ticket.UserId,
                RaffleId = ticket.RaffleId,
                IsWinner = ticket.IsWinner,
                PurchaseDate = ticket.PurchaseDate,
                CreatedAt = ticket.CreatedAt,
                IsEnabled = ticket.IsEnabled
            };
        }
    }
}
