using Raffle.Aplication.DTOs.Prize;
using Raffle.Domain.Entities;
using Raffle.Domain.Enums;
using Raffle.Infrastructure.Data;
using Raffle.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Services
{

    public class PrizeService : IPrizeService
    {
        private readonly RaffleDbContext _context;
        private readonly IPrizeRepository _prizeRepository;
        private readonly IRaffleRepository _RaffleRepository;

        private readonly List<Prize> _prizes = new(); // Banco de dados simulado, substitua por um contexto de banco real.
        public PrizeService(RaffleDbContext context, IPrizeRepository prizeRepository, IRaffleRepository raffleRepository)
        {
            _context = context;
            _prizeRepository = prizeRepository;
            _RaffleRepository = raffleRepository;
        }

        public async Task<Prize> CreatePrizeAsync(CreatePrizeDtoRequest dtoRequest)
        {
            var newPrize = new Prize
            {
                Id = Guid.NewGuid().ToString(),
                Title = dtoRequest.Title,
                Description = dtoRequest.Description,
                Value = dtoRequest.Value,
                Quantity = dtoRequest.Quantity,
                Type = dtoRequest.Type,
                IsEnabled = true
            };

            _prizes.Add(newPrize);

            return await Task.FromResult(newPrize); // Simula operação assíncrona
        }

        public async Task<IEnumerable<Prize>> GetAllPrizesAsync()
        {
            // Aqui você faria uma consulta no banco de dados, mas estamos usando uma lista simulada.
            return await Task.FromResult(_prizes.AsEnumerable()); // Simula operação assíncrona
        }

        public async Task<Prize> GetPrizeByIdAsync(string prizeId)
        {
            var prize = _prizes.FirstOrDefault(p => p.Id == prizeId);

            return await Task.FromResult(prize); // Simula operação assíncrona
        }

        public async Task<IEnumerable<Prize>> GetPrizesByRaffleIdAsync(string raffleId)
        {
            var prizes = _prizes.Where(p => p.RaffleId == raffleId).ToList();

            return await Task.FromResult(prizes); // Simula operação assíncrona
        }

        public async Task<Prize> UpdatePrizeAsync(string prizeId, UpdatePrizeDtoRequest dtoRequest)
        {
            var prize = _prizes.FirstOrDefault(p => p.Id == prizeId);

            if (prize == null) throw new KeyNotFoundException("Prêmio não encontrado.");

            prize.Title = dtoRequest.Title;
            prize.Description = dtoRequest.Description;
            prize.Value = dtoRequest.Value;
            prize.Quantity = dtoRequest.Quantity;
            prize.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(prize); // Simula operação assíncrona
        }
        public async Task<bool> DeletePrizeAsync(string aId)
        {
            var prize = _prizes.FirstOrDefault(p => p.Id == aId);

            if (prize == null)
                return false;

            _prizes.Remove(prize);

            return true; // Ou await Task.CompletedTask seguido de return true, se mantiver a simulação
        }

        public async Task<PrizeDtoResponse> AddPrizeToRaffleAsync(string raffleId, CreatePrizeDtoRequest dtoRequest)
        {
            var raffle = await _RaffleRepository.GetByIdAsync(raffleId);

            if (raffle == null)
                throw new KeyNotFoundException("Rifa não encontrada.");

            var prize = new Prize
            {
                Id = Guid.NewGuid().ToString(),
                Title = dtoRequest.Title,
                Description = dtoRequest.Description,
                Value = dtoRequest.Value,
                ImageUrl = dtoRequest.ImageUrl,
                Quantity = dtoRequest.Quantity,
                Type = dtoRequest.Type,
                RaffleId = raffleId,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AddAsync(prize);

            return new PrizeDtoResponse(prize);
        }

        public async Task RemovePrizeFromRaffleAsync(string raffleId, string prizeId)
        {
            var prize = await _prizeRepository.GetByIdAsync(prizeId);
            if (prize == null || prize.RaffleId != raffleId)
                throw new KeyNotFoundException("Prêmio ou rifa não encontrada.");

            await _prizeRepository.DeleteAsync(prizeId);
        }

        public async Task<PrizeDtoResponse> UpdatePrizeInRaffleAsync(string raffleId, string prizeId, UpdatePrizeDtoRequest dtoRequest)
        {
            var prize = await _prizeRepository.GetByIdAsync(prizeId);
            if (prize == null || prize.RaffleId != raffleId)
                throw new KeyNotFoundException("Prêmio ou rifa não encontrada.");

            prize.Title = dtoRequest.Title;
            prize.Description = dtoRequest.Description;
            prize.Value = dtoRequest.Value;
            prize.ImageUrl = dtoRequest.ImageUrl;
            prize.Quantity = dtoRequest.Quantity;

            // Conversão explícita do valor inteiro para o enum PrizeType
            prize.Type = (PrizeType)dtoRequest.Type;

            prize.UpdatedAt = DateTime.UtcNow;

            await _prizeRepository.UpdateAsync(prize);
            return new PrizeDtoResponse(prize);
        }

    }
}
