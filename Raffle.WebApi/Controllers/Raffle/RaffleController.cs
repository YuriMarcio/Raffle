using Microsoft.AspNetCore.Mvc;
using Raffle.Aplication.DTOs.RaffleDto;
using Raffle.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raffle.Aplication.DTOs.Prize;

namespace Raffle.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaffleController : ControllerBase
    {
        private readonly IRaffleService _raffleService;
        private readonly IPrizeService _prizeService;

        public RaffleController(IRaffleService raffleService, IPrizeService prizeService)
        {
            _raffleService = raffleService;
            _prizeService = prizeService;
        }

        /// <summary>
        /// Lista todas as rifas disponíveis.
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetAllRaffles()
        {
            var result = await _raffleService.GetAllRafflesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtém os detalhes de uma rifa específica.
        /// </summary>
        /// <param name="id">ID da rifa</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaffleById(string id)
        {
            var result = await _raffleService.GetRaffleByIdAsync(id);
            if (result == null)
                return NotFound("Rifa não encontrada.");

            return Ok(result);
        }

        /// <summary>
        /// Cria uma nova rifa.
        /// </summary>
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

        /// <summary>
        /// Atualiza uma rifa existente.
        /// </summary>
        /// <param name="id">ID da rifa</param>
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

        /// <summary>
        /// Exclui uma rifa.
        /// </summary>
        /// <param name="id">ID da rifa</param>
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

        /// <summary>
        /// Lista os prêmios vinculados a uma rifa específica.
        /// </summary>
        /// <param name="raffleId">ID da rifa</param>
        [HttpGet("{raffleId}/prizes")]
        public async Task<IActionResult> GetRafflePrizes(string raffleId)
        {
            var prizes = await _prizeService.GetPrizesByRaffleIdAsync(raffleId);
            return Ok(prizes);
        }

        /// <summary>
        /// Adiciona um prêmio a uma rifa específica.
        /// </summary>
        /// <param name="raffleId">ID da rifa</param>
        [HttpPost("{raffleId}/prizes")]
        public async Task<IActionResult> AddPrizeToRaffle(string raffleId, [FromBody] CreatePrizeDtoRequest dtoRequest)
        {
            try
            {
                PrizeDtoResponse result = await _prizeService.AddPrizeToRaffleAsync(raffleId, dtoRequest);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Rifa não encontrada.");
            }
        }

        /// <summary>
        /// Remove um prêmio de uma rifa.
        /// </summary>
        /// <param name="raffleId">ID da rifa</param>
        /// <param name="prizeId">ID do prêmio</param>
        [HttpDelete("{raffleId}/prizes/{prizeId}")]
        public async Task<IActionResult> RemovePrizeFromRaffle(string raffleId, string prizeId)
        {
            try
            {
                await _prizeService.RemovePrizeFromRaffleAsync(raffleId, prizeId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Prêmio ou rifa não encontrada.");
            }
        }

        /// <summary>
        /// Atualiza os detalhes de um prêmio vinculado a uma rifa.
        /// </summary>
        /// <param name="raffleId">ID da rifa</param>
        /// <param name="prizeId">ID do prêmio</param>
        [HttpPut("{raffleId}/prizes/{prizeId}")]
        public async Task<IActionResult> UpdatePrizeInRaffle(string raffleId, string prizeId, [FromBody] UpdatePrizeDtoRequest dtoRequest)
        {
            try
            {
                var result = await _prizeService.UpdatePrizeInRaffleAsync(raffleId, prizeId, dtoRequest);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Prêmio ou rifa não encontrada.");
            }
        }
    }
}
