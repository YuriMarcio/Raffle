using Raffle.Application.DTOs.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Retorna todos os usuários cadastrados.
        /// </summary>
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        /// <summary>
        /// Retorna um usuário pelo ID.
        /// </summary>
        Task<UserResponseDto?> GetUserByIdAsync(string id);

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto userDto);

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        Task<UserResponseDto?> UpdateUserAsync(string id, UpdateUserRequestDto userDto);

        /// <summary>
        /// Remove um usuário do sistema.
        /// </summary>
        Task<bool> DeleteUserAsync(string id);
    }
}
