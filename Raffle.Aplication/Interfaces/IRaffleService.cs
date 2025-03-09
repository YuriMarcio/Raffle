using Raffle.Aplication.DTOs.AutenticationDto;
using Raffle.Aplication.DTOs.RaffleDto;
using Raffle.Domain.Entities.Raffle;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Services
{
    public interface IRaffleService
    {
        Task<CreateRaffleDtoResponse> CreateRaffleAsync(CreateRaffleDtoRequest dtoRequest);
        Task DeleteRaffleAsync(string aId);
        Task<IEnumerable<RaffleEntity>> GetAllRafflesAsync();
        Task<RaffleDto> GetRaffleByIdAsync(string aID);
        Task<RaffleDto> UpdateRaffleAsync(string aID, UpdateRaffleDtoRequest dtoRequest);
    }
}
