using Microsoft.AspNetCore.Mvc;
using Raffle.Aplication.DTOs.RaffleDto;
using Raffle.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raffle.Aplication.DTOs.Prize;
using Raffle.Aplication.Interfaces;
using Raffle.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using System.Linq;

namespace Raffle.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Route("api/raffles")]
    [EnableCors("AllowAll")]
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
        /// Lista todos os sorteios promocionais disponíveis.
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetAllRaffles()
        {
            var result = await _raffleService.GetAllRafflesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Lista todos os sorteios promocionais com filtros e paginação.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRaffles(
            [FromQuery] string category = null,
            [FromQuery] string status = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string search = null,
            [FromQuery] string organizationId = null,
            [FromQuery] string sortBy = "date",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12)
        {
            var raffles = await _raffleService.GetAllRafflesAsync();
            var filteredRaffles = raffles.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(category))
                filteredRaffles = filteredRaffles.Where(r => r.Type.ToString() == category);

            if (!string.IsNullOrEmpty(status))
                filteredRaffles = filteredRaffles.Where(r => r.Status.ToString() == status);

            if (minPrice.HasValue)
                filteredRaffles = filteredRaffles.Where(r => r.TicketPrice >= minPrice.Value);

            if (maxPrice.HasValue)
                filteredRaffles = filteredRaffles.Where(r => r.TicketPrice <= maxPrice.Value);

            if (!string.IsNullOrEmpty(search))
                filteredRaffles = filteredRaffles.Where(r =>
                    r.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    r.Description.Contains(search, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(organizationId))
                filteredRaffles = filteredRaffles.Where(r => r.UserId == organizationId);

            // Apply sorting
            filteredRaffles = sortBy switch
            {
                "price" => filteredRaffles.OrderBy(r => r.TicketPrice),
                "popularity" => filteredRaffles.OrderByDescending(r => r.TicketsSold),
                "ending" => filteredRaffles.OrderBy(r => r.EndDate),
                _ => filteredRaffles.OrderBy(r => r.StartDate)
            };

            var total = filteredRaffles.Count();
            var paginatedRaffles = filteredRaffles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new { raffles = paginatedRaffles, total });
        }

        /// <summary>
        /// Obtém os detalhes de um sorteio promocional específico.
        /// </summary>
        /// <param name="id">ID do sorteio promocional</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaffleById(string id)
        {
            try
            {
                var raffle = await _raffleService.GetRaffleByIdAsync(id);
                if (raffle == null)
                    return NotFound("Sorteio promocional não encontrado.");

                return Ok(raffle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        [HttpGet("test")]
        public IActionResult TestEndpoint()
        {
            return Ok(new { message = "CORS test successful", timestamp = DateTime.Now });
        }

        /// <summary>
        /// Busca sorteio promocional por link único.
        /// </summary>
        [HttpGet("link/{uniqueLink}")]
        public async Task<IActionResult> GetRaffleByLink(string uniqueLink)
        {
            var raffle = await _raffleService.GetByUniqueLinkAsync(uniqueLink);
            if (raffle == null)
                return NotFound("Sorteio promocional não encontrado.");

            return Ok(raffle);
        }

        /// <summary>
        /// Busca sorteios promocionais ativos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveRaffles()
        {
            var raffles = await _raffleService.GetAllRafflesAsync();
            var activeRaffles = raffles.Where(r =>
                r.Status == Domain.Enums.RaffleStatus.Active &&
                r.IsEnable &&
                r.EndDate > DateTime.UtcNow);

            return Ok(activeRaffles);
        }

        /// <summary>
        /// Busca sorteios promocionais em destaque.
        /// </summary>
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedRaffles()
        {
            var raffles = await _raffleService.GetAllRafflesAsync();
            var featuredRaffles = raffles
                .Where(r => r.Status == Domain.Enums.RaffleStatus.Active && r.IsEnable)
                .OrderByDescending(r => r.TicketsSold)
                .Take(6);

            return Ok(featuredRaffles);
        }

        /// <summary>
        /// Busca sorteios promocionais terminando em breve.
        /// </summary>
        [HttpGet("ending-soon")]
        public async Task<IActionResult> GetEndingSoonRaffles()
        {
            var raffles = await _raffleService.GetAllRafflesAsync();
            var endingSoon = raffles
                .Where(r => r.Status == Domain.Enums.RaffleStatus.Active &&
                       r.IsEnable &&
                       r.EndDate.HasValue &&
                       r.EndDate.Value > DateTime.UtcNow &&
                       r.EndDate.Value <= DateTime.UtcNow.AddDays(7))
                .OrderBy(r => r.EndDate);

            return Ok(endingSoon);
        }

        /// <summary>
        /// Busca sorteios promocionais por categoria.
        /// </summary>
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetRafflesByCategory(string category)
        {
            var raffles = await _raffleService.GetAllRafflesAsync();
            var categoryRaffles = raffles.Where(r =>
                r.Type.ToString().Equals(category, StringComparison.OrdinalIgnoreCase) &&
                r.Status == Domain.Enums.RaffleStatus.Active &&
                r.IsEnable);

            return Ok(categoryRaffles);
        }

        /// <summary>
        /// Busca sorteios promocionais de uma organização.
        /// </summary>
        [HttpGet("organization/{organizationId}")]
        public async Task<IActionResult> GetRafflesByOrganization(string organizationId)
        {
            var raffles = await _raffleService.GetAllRafflesAsync();
            var orgRaffles = raffles.Where(r => r.UserId == organizationId);

            return Ok(orgRaffles);
        }

        /// <summary>
        /// Busca meus sorteios promocionais.
        /// </summary>
        [HttpGet("my-raffles")]
        public async Task<IActionResult> GetMyRaffles()
        {
            // TODO: Get current user ID from authentication context
            var userId = "current-user-id"; // This should come from User.Identity
            var raffles = await _raffleService.GetAllRafflesAsync();
            var myRaffles = raffles.Where(r => r.UserId == userId);

            return Ok(myRaffles);
        }

        /// <summary>
        /// Busca números disponíveis de um sorteio promocional.
        /// </summary>
        [HttpGet("{raffleId}/available-numbers")]
        public async Task<IActionResult> GetAvailableNumbers(string raffleId)
        {
            try
            {
                var raffle = await _raffleService.GetRaffleByIdAsync(raffleId);
                if (raffle == null)
                    return NotFound("Sorteio promocional não encontrado.");

                // TODO: Implement proper available numbers logic
                var availableNumbers = Enumerable.Range(1, 100).ToList();
                return Ok(availableNumbers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Busca estatísticas de um sorteio promocional.
        /// </summary>
        [HttpGet("{id}/stats")]
        public async Task<IActionResult> GetRaffleStats(string id)
        {
            var raffle = await _raffleService.GetRaffleByIdAsync(id);
            if (raffle == null)
                return NotFound("Sorteio promocional não encontrado.");

            var stats = new
            {
                totalSold = raffle.TicketsSold,
                totalRevenue = raffle.TicketsSold * raffle.TicketPrice,
                participantsCount = raffle.RaffleClients?.Count ?? 0,
                salesByDay = new[]
                {
                    new { date = DateTime.UtcNow.AddDays(-6).ToString("yyyy-MM-dd"), count = 10, revenue = 500 },
                    new { date = DateTime.UtcNow.AddDays(-5).ToString("yyyy-MM-dd"), count = 15, revenue = 750 },
                    new { date = DateTime.UtcNow.AddDays(-4).ToString("yyyy-MM-dd"), count = 20, revenue = 1000 },
                    new { date = DateTime.UtcNow.AddDays(-3).ToString("yyyy-MM-dd"), count = 18, revenue = 900 },
                    new { date = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd"), count = 25, revenue = 1250 },
                    new { date = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd"), count = 30, revenue = 1500 },
                    new { date = DateTime.UtcNow.ToString("yyyy-MM-dd"), count = 22, revenue = 1100 }
                }
            };

            return Ok(stats);
        }

        /// <summary>
        /// Reserva números de bilhetes para compra.
        /// </summary>
        [HttpPost("{raffleId}/reserve-tickets")]
        public async Task<IActionResult> ReserveTickets(string raffleId, [FromBody] ReserveTicketsRequest request)
        {
            try
            {
                var raffle = await _raffleService.GetRaffleByIdAsync(raffleId);
                if (raffle == null)
                    return NotFound("Sorteio promocional não encontrado.");

                // TODO: Implement actual reservation logic
                var reservation = new
                {
                    reservationId = Guid.NewGuid().ToString(),
                    raffleId = raffleId,
                    ticketNumbers = request.TicketNumbers,
                    totalPrice = request.TicketNumbers.Count * raffle.TicketPrice,
                    expiresAt = DateTime.UtcNow.AddMinutes(10), // 10 minutes reservation
                    status = "pending"
                };

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Confirma a compra de bilhetes.
        /// </summary>
        [HttpPost("{raffleId}/purchase-tickets")]
        public async Task<IActionResult> PurchaseTickets(string raffleId, [FromBody] PurchaseTicketsRequest request)
        {
            try
            {
                var raffle = await _raffleService.GetRaffleByIdAsync(raffleId);
                if (raffle == null)
                    return NotFound("Sorteio promocional não encontrado.");

                // TODO: Implement actual purchase logic with payment processing
                var purchase = new
                {
                    purchaseId = Guid.NewGuid().ToString(),
                    raffleId = raffleId,
                    ticketNumbers = request.TicketNumbers,
                    totalAmount = request.TicketNumbers.Count * raffle.TicketPrice,
                    paymentMethod = request.PaymentMethod,
                    customerInfo = request.CustomerInfo,
                    purchaseDate = DateTime.UtcNow,
                    status = "completed",
                    paymentDetails = new
                    {
                        pixQrCode = request.PaymentMethod == "pix" ? "00020126360014BR.GOV.BCB.PIX0114+5511999999999520400005303986540520.005802BR5924SORTIO MARKETPLACE LTDA6009SAO PAULO62140510ABC12345676304ABCD" : null,
                        pixKey = request.PaymentMethod == "pix" ? "pix@sortio.com.br" : null,
                        boletoUrl = request.PaymentMethod == "boleto" ? "https://boleto.example.com/123456" : null
                    }
                };

                return Ok(purchase);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class ReserveTicketsRequest
        {
            public List<int> TicketNumbers { get; set; }
        }

        public class PurchaseTicketsRequest
        {
            public List<int> TicketNumbers { get; set; }
            public string PaymentMethod { get; set; }
            public CustomerInfo CustomerInfo { get; set; }
        }

        public class CustomerInfo
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Cpf { get; set; }
        }

        /// <summary>
        /// Busca ganhadores recentes.
        /// </summary>
        [HttpGet("winners")]
        public async Task<IActionResult> GetWinners([FromQuery] int? limit = null)
        {
            var raffles = await _raffleService.GetAllRafflesAsync();
            var winners = new List<object>();

            // TODO: Implement proper winners logic
            // For now, return mock data
            winners.Add(new
            {
                raffleId = "1",
                raffleTitle = "Honda Civic 2024",
                winnerName = "João Silva",
                winnerCity = "São Paulo",
                winnerState = "SP",
                ticketNumber = "1234",
                prize = "Honda Civic 2024 0km",
                prizeValue = 180000,
                drawDate = DateTime.UtcNow.AddDays(-7).ToString("yyyy-MM-dd"),
                organizationName = "Instituto Esperança"
            });

            if (limit.HasValue)
                winners = winners.Take(limit.Value).ToList();

            return Ok(winners);
        }

        /// <summary>
        /// Busca categorias disponíveis.
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var raffles = await _raffleService.GetAllRafflesAsync();
            var categories = Enum.GetValues<Domain.Enums.RaffleType>()
                .Select(type => new
                {
                    id = type.ToString(),
                    name = GetCategoryName(type),
                    icon = GetCategoryIcon(type),
                    count = raffles.Count(r => r.Type == type && r.Status == Domain.Enums.RaffleStatus.Active)
                });

            return Ok(categories);
        }

        private string GetCategoryName(Domain.Enums.RaffleType type)
        {
            return type switch
            {
                Domain.Enums.RaffleType.Car => "Carros e Motos",
                Domain.Enums.RaffleType.Electronic => "Eletrônicos",
                Domain.Enums.RaffleType.Property => "Imóveis",
                Domain.Enums.RaffleType.Travel => "Viagens",
                Domain.Enums.RaffleType.Experience => "Experiências",
                Domain.Enums.RaffleType.Other => "Outros",
                _ => type.ToString()
            };
        }

        private string GetCategoryIcon(Domain.Enums.RaffleType type)
        {
            return type switch
            {
                Domain.Enums.RaffleType.Car => "car",
                Domain.Enums.RaffleType.Electronic => "smartphone",
                Domain.Enums.RaffleType.Property => "home",
                Domain.Enums.RaffleType.Travel => "plane",
                Domain.Enums.RaffleType.Experience => "gift",
                Domain.Enums.RaffleType.Other => "package",
                _ => "help-circle"
            };
        }

        /// <summary>
        /// Cria um novo sorteio promocional.
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
        /// Atualiza um sorteio promocional existente.
        /// </summary>
        /// <param name="id">ID do sorteio promocional</param>
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
                return NotFound("Sorteio promocional não encontrado.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um sorteio promocional.
        /// </summary>
        /// <param name="id">ID do sorteio promocional</param>
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
                return NotFound("Sorteio promocional não encontrado.");
            }
        }

        /// <summary>
        /// Lista os prêmios vinculados a um sorteio promocional específico.
        /// </summary>
        /// <param name="raffleId">ID do sorteio promocional</param>
        [HttpGet("{raffleId}/prizes")]
        public async Task<IActionResult> GetRafflePrizes(string raffleId)
        {
            var prizes = await _prizeService.GetPrizesByRaffleIdAsync(raffleId);
            return Ok(prizes);
        }

        /// <summary>
        /// Adiciona um prêmio a um sorteio promocional específico.
        /// </summary>
        /// <param name="raffleId">ID do sorteio promocional</param>
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
                return NotFound("Sorteio promocional não encontrado.");
            }
        }

        /// <summary>
        /// Remove um prêmio de um sorteio promocional.
        /// </summary>
        /// <param name="raffleId">ID do sorteio promocional</param>
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
                return NotFound("Prêmio ou sorteio promocional não encontrado.");
            }
        }

        /// <summary>
        /// Atualiza os detalhes de um prêmio vinculado a um sorteio promocional.
        /// </summary>
        /// <param name="raffleId">ID do sorteio promocional</param>
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
                return NotFound("Prêmio ou sorteio promocional não encontrado.");
            }
        }
    }
}
