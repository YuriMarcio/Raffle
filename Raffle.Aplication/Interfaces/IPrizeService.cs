using Raffle.Aplication.DTOs.Prize;
using Raffle.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Aplication.Interfaces
{
    public interface IPrizeService
    {
          /// <summary>
        /// Adiciona um prêmio a um sorteio promocional específico.
        /// </summary>
        /// <param name="raffleId">ID do sorteio promocional.</param>
        /// <param name="dtoRequest">Dados do prêmio a ser adicionado.</param>
        /// <returns>Detalhes do prêmio criado.</returns>
        Task<PrizeDtoResponse> AddPrizeToRaffleAsync(string raffleId, CreatePrizeDtoRequest dtoRequest);

        /// <summary>
        /// Remove um prêmio de um sorteio promocional específico.
        /// </summary>
        /// <param name="raffleId">ID do sorteio promocional.</param>
        /// <param name="prizeId">ID do prêmio.</param>
        Task RemovePrizeFromRaffleAsync(string raffleId, string prizeId);

        /// <summary>
        /// Atualiza os detalhes de um prêmio vinculado a um sorteio promocional.
        /// </summary>
        /// <param name="raffleId">ID do sorteio promocional.</param>
        /// <param name="prizeId">ID do prêmio.</param>
        /// <param name="dtoRequest">Dados atualizados do prêmio.</param>
        /// <returns>Detalhes do prêmio atualizado.</returns>
        Task<PrizeDtoResponse> UpdatePrizeInRaffleAsync(string raffleId, string prizeId, UpdatePrizeDtoRequest dtoRequest);
        Task<IEnumerable<Prize>> GetAllPrizesAsync();
        Task<Prize> CreatePrizeAsync(CreatePrizeDtoRequest dtoRequest);
        Task<Prize> GetPrizeByIdAsync(string prizeId);
        Task<IEnumerable<Prize>> GetPrizesByRaffleIdAsync(string raffleId);
        Task<Prize> UpdatePrizeAsync(string prizeId, UpdatePrizeDtoRequest dtoRequest);
        Task <bool> DeletePrizeAsync(string aId);
    }
}
