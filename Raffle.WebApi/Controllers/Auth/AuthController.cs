using Microsoft.AspNetCore.Mvc;
using Raffle.Aplication.DTOs.AutenticationDto;
using Raffle.Application.Interfaces;
using Raffle.Infrastructure.Services;
using Raffle.Aplication.Services;

namespace Raffle.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDocumentVerificationService _documentVerificationService;

        public AuthController(IAuthService authService, IDocumentVerificationService documentVerificationService)
        {
            _authService = authService;
            _documentVerificationService = documentVerificationService;
        }

        // Login de usuário
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDtoRequest aDtoRequest)
        {
            try
            {
                var token = await _authService.LoginAsync(aDtoRequest.Email, aDtoRequest.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Credenciais inválidas.");
            }
        }

        // ENDPOINT REMOVIDO - Lei 5.768/71 proíbe sorteios promocionais por CPF
        // Apenas empresas (CNPJ) podem realizar sorteios promocionais
        /*
        [HttpPost("register/cpf")]
        public async Task<IActionResult> RegisterCPF([FromBody] RegisterCPFRequest request)
        {
            return BadRequest(new {
                message = "Cadastro por CPF não permitido. Conforme Lei 5.768/71, apenas empresas (CNPJ) podem realizar sorteios promocionais.",
                legalReference = "Lei 5.768/71 - Art. 1º"
            });
        }
        */

        // Registro de usuário PJ (CNPJ)
        [HttpPost("register/cnpj")]
        public async Task<IActionResult> RegisterCNPJ([FromBody] RegisterCNPJRequest request)
        {
            try
            {
                // Valida o CNPJ
                if (!_documentVerificationService.ValidateCNPJ(request.CNPJ))
                {
                    return BadRequest(new { message = "CNPJ inválido" });
                }

                // Valida o CPF do responsável
                if (!_documentVerificationService.ValidateCPF(request.ResponsibleCPF))
                {
                    return BadRequest(new { message = "CPF do responsável inválido" });
                }

                // Verifica o CNPJ (simulado por enquanto)
                var cnpjVerification = await _documentVerificationService.VerifyCNPJ(request.CNPJ, request.CompanyName);

                if (cnpjVerification.Status == "IRREGULAR")
                {
                    return BadRequest(new {
                        message = "CNPJ com pendências fiscais. Entre em contato com o suporte.",
                        status = cnpjVerification.Status
                    });
                }

                // Verifica o CPF do responsável
                var cpfVerification = await _documentVerificationService.VerifyCPF(request.ResponsibleCPF, request.ResponsibleName);

                if (cpfVerification.Status != "REGULAR")
                {
                    return BadRequest(new {
                        message = "CPF do responsável com pendências.",
                        status = cpfVerification.Status
                    });
                }

                // Registra o usuário
                var userId = await _authService.RegisterCNPJAsync(request);

                return Ok(new {
                    message = "Cadastro empresarial realizado com sucesso!",
                    userId = userId,
                    cnpjStatus = cnpjVerification.Status,
                    cpfStatus = cpfVerification.Status
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao processar cadastro", error = ex.Message });
            }
        }

        // Verificar status do documento
        [HttpGet("verify-document/{userId}")]
        public async Task<IActionResult> VerifyDocumentStatus(string userId)
        {
            try
            {
                var status = await _authService.GetDocumentVerificationStatus(userId);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
