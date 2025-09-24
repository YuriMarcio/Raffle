using Microsoft.AspNetCore.Mvc;
using Raffle.Application.Interfaces;
using Raffle.Domain.Entities.Raffle;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Raffle.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IRaffleService _raffleService;

        public PublicController(IRaffleService raffleService)
        {
            _raffleService = raffleService;
        }

        [HttpGet("raffle/{uniqueLink}")]
        public async Task<ActionResult<RaffleEntity>> GetRaffleByUniqueLink(string uniqueLink)
        {
            try
            {
                var raffle = await _raffleService.GetByUniqueLinkAsync(uniqueLink);
                if (raffle == null)
                {
                    return NotFound("Sorteio promocional não encontrado");
                }

                return Ok(raffle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("raffle/{uniqueLink}/available-tickets")]
        public async Task<ActionResult<List<int>>> GetAvailableTickets(string uniqueLink)
        {
            try
            {
                var availableTickets = await _raffleService.GetAvailableTicketsByUniqueLinkAsync(uniqueLink);
                return Ok(availableTickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("raffle/{uniqueLink}/purchase")]
        public async Task<ActionResult> PurchaseTickets(string uniqueLink, [FromBody] PurchaseRequest request)
        {
            try
            {
                var result = await _raffleService.PurchaseTicketsPublicAsync(uniqueLink, request.TicketNumbers, request.CustomerInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("raffle/{uniqueLink}/ticket/{ticketNumber}")]
        public async Task<ActionResult> GetTicketInfo(string uniqueLink, int ticketNumber)
        {
            try
            {
                var ticketInfo = await _raffleService.GetTicketByUniqueLinkAsync(uniqueLink, ticketNumber);
                return Ok(ticketInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class PurchaseRequest
    {
        public List<int> TicketNumbers { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
    }

    public class CustomerInfo
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
    }
}