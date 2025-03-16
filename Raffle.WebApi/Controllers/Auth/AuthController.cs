using Microsoft.AspNetCore.Mvc;
using Raffle.Aplication.DTOs.AutenticationDto;
using Raffle.Application.Interfaces;
using Raffle.Infrastructure.Services;

namespace Raffle.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
    }
}
