using Microsoft.AspNetCore.Mvc;
using Raffle.Aplication.DTOs.Payment;
using Raffle.Application.Services;
using Raffle.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Raffle.WebApi.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Cria um novo pagamento.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            var result = await _paymentService.CreatePaymentAsync(request.UserId, request.Amount);
            return CreatedAtAction(nameof(GetPaymentById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Obtém um pagamento por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound("Pagamento não encontrado.");

            return Ok(payment);
        }

        /// <summary>
        /// Obtém todos os pagamentos de um usuário.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUser(string userId)
        {
            var payments = await _paymentService.GetPaymentsByUserAsync(userId);
            return Ok(payments);
        }

        /// <summary>
        /// Atualiza o status de um pagamento.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] PaymentStatusUpdateRequest request)
        {
            var updatedPayment = await _paymentService.UpdatePaymentStatusAsync(id, request.Status);
            if (updatedPayment == null)
                return NotFound("Pagamento não encontrado.");

            return Ok(updatedPayment);
        }
    }
}
