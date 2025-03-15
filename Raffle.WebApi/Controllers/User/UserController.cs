using Microsoft.AspNetCore.Mvc;
using Raffle.Aplication;
using Raffle.Application.DTOs.User;
using Raffle.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Construtor do UserController.
        /// </summary>
        /// <param name="userService">Serviço de gerenciamento de usuários.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Lista todos os usuários cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserRequestDto userDto)
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDto>> Update(string id, [FromBody] UpdateUserRequestDto userDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, userDto);

            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        /// <summary>
        /// Remove um usuário do sistema.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _userService.DeleteUserAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
