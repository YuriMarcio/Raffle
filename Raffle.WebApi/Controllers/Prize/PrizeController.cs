using LeadSoft.Common.Library.Extensions;
using Microsoft.AspNetCore.Mvc;
using Raffle.Aplication.DTOs.Prize;
using Raffle.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.WebApi.Controllers.Prize
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de prêmios.
    /// </summary>
    [ApiController]
    [Route("api/prizes")]
    public class PrizeController : ControllerBase
    {
        private readonly IPrizeService _prizeService;

        /// <summary>
        /// Construtor do PrizeController.
        /// </summary>
        /// <param name="prizeService">Serviço de gerenciamento de prêmios.</param>
        public PrizeController(IPrizeService prizeService)
        {
            _prizeService = prizeService;
        }

        /// <summary>
        /// Obtém a lista de todos os prêmios cadastrados.
        /// </summary>
        /// <returns>Lista de prêmios.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrizeDto>>> GetAll()
        {
            var prizes = await _prizeService.GetAllPrizesAsync();
            return Ok(prizes);
        }

        /// <summary>
        /// Obtém um prêmio específico pelo ID.
        /// </summary>
        /// <param name="id">ID do prêmio.</param>
        /// <returns>O prêmio correspondente ao ID informado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PrizeDto>> GetById(int id)
        {
            var prize = await _prizeService.GetPrizeByIdAsync(id.ToString());
            if (prize == null)
                return NotFound("Prêmio não encontrado");

            return Ok(prize);
        }

        /// <summary>
        /// Cria um novo prêmio.
        /// </summary>
        /// <param name="dto">Dados do prêmio a ser criado.</param>
        /// <returns>O prêmio recém-criado.</returns>
        [HttpPost]
        public async Task<ActionResult<PrizeDto>> Create([FromBody] CreatePrizeDtoRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("Nome do prêmio é obrigatório");

            var newPrize = await _prizeService.CreatePrizeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newPrize.Id }, newPrize);
        }

        /// <summary>
        /// Atualiza os dados de um prêmio existente.
        /// </summary>
        /// <param name="id">ID do prêmio a ser atualizado.</param>
        /// <param name="dto">Novos dados do prêmio.</param>
        /// <returns>Sem conteúdo caso a atualização seja bem-sucedida.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdatePrizeDtoRequest dto)
        {
            var data = await _prizeService.UpdatePrizeAsync(id.ToString(), dto);

            if (data.IsNotNull())
                return NotFound("Prêmio não encontrado");

            return NoContent();
        }

        /// <summary>
        /// Remove um prêmio do sistema.
        /// </summary>
        /// <param name="id">ID do prêmio a ser removido.</param>
        /// <returns>Sem conteúdo caso a remoção seja bem-sucedida.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var success = await _prizeService.DeletePrizeAsync(id);
            if (!success)
                return NotFound("Prêmio não encontrado");

            return NoContent();
        }
    }
}
