using Microsoft.AspNetCore.Mvc;
using Raffle.Application.DTOs.Requests;
using Raffle.Application.DTOs.Responses;
using Raffle.Application.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.API.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketResponse>>> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketResponse>> GetTicketById(string id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null) return NotFound("Ticket não encontrado.");
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<TicketResponse>> CreateTicket([FromBody] TicketDTORequest request)
        {
            if (request == null) return BadRequest("Dados inválidos.");
            var newTicket = await _ticketService.CreateTicketAsync(request);
            return CreatedAtAction(nameof(GetTicketById), new { id = newTicket.Id }, newTicket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketResponse>> UpdateTicket(string id, [FromBody] TicketDTORequest request)
        {
            var updatedTicket = await _ticketService.UpdateTicketAsync(id, request);
            if (updatedTicket == null) return NotFound("Ticket não encontrado.");
            return Ok(updatedTicket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(string id)
        {
            var deleted = await _ticketService.DeleteTicketAsync(id);
            if (!deleted) return NotFound("Ticket não encontrado.");
            return NoContent();
        }
    }
}
