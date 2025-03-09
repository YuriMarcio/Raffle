using Microsoft.AspNetCore.Mvc;
using Raffle.Aplication.DTOs.RaffleDto;
using Raffle.Infrastructure.Services;

namespace Raffle.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaffleController : ControllerBase
    {
        private readonly IRaffleService _raffleService;
        public RaffleController(IRaffleService raffleService)
        {
            _raffleService = raffleService;
        }

        // Criar uma nova rifa
        [HttpPost("create")]
        public async Task<IActionResult> CreateRaffle([FromBody] CreateRaffleDtoRequest dtoRequest)
        {
            try
            {
                var result = await _raffleService.CreateRaffleAsync(dtoRequest);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Obter todas as rifas
        [HttpGet("list")]
        public async Task<IActionResult> GetAllRaffles()
        {
            var result = await _raffleService.GetAllRafflesAsync();
            return Ok(result);
        }

        // Obter detalhes de uma rifa específica
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaffleById(string id)
        {
            var result = await _raffleService.GetRaffleByIdAsync(id);
            if (result == null)
                return NotFound("Rifa não encontrada.");

            return Ok(result);
        }

        // Atualizar uma rifa
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRaffle(string id, [FromBody] UpdateRaffleDtoRequest dtoRequest)
        {
            try
            {
                var result = await _raffleService.UpdateRaffleAsync(id, dtoRequest);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Rifa não encontrada.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Excluir uma rifa
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRaffle(string id)
        {
            try
            {
                await _raffleService.DeleteRaffleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Rifa não encontrada.");
            }
        }
    }
}
