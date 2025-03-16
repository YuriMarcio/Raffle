using Raffle.Application.DTOs.User;
using Raffle.Application;
using Raffle.Application.Interfaces;
using Raffle.Domain.Entities;
using Raffle.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados.
        /// </summary>
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                IsEnabled = u.IsEnabled,
                IsAdmin = u.IsAdmin
            });
        }

        /// <summary>
        /// Retorna um usuário pelo ID.
        /// </summary>
        public async Task<UserResponseDto?> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsEnabled = user.IsEnabled,
                IsAdmin = user.IsAdmin
            };
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto userDto)
        {
            //var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == aDtoRequest.Email);
            //if (existingUser != null)
            //{
            //    throw new InvalidOperationException("Usuário já existe.");
            //}

            //var user = new User
            //{
            //    Name = aDtoRequest.Name,
            //    Email = aDtoRequest.Email,
            //    IsAdmin = aDtoRequest.IsAdmin,
            //    Password = BCrypt.Net.BCrypt.HashPassword(aDtoRequest.Password)
            //};

            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();

            //return "Usuário registrado com sucesso.";
            User newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Phone = userDto.Phone,
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password, // Hash de senha deve ser aplicado aqui!
                IsEnabled = true,
                IsAdmin = userDto.IsAdmin
            };

            await _userRepository.AddAsync(newUser);

            return new UserResponseDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
                IsEnabled = newUser.IsEnabled,
                IsAdmin = newUser.IsAdmin
            };
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        public async Task<UserResponseDto?> UpdateUserAsync(string id, UpdateUserRequestDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            user.Name = userDto.Name ?? user.Name;
            user.Email = userDto.Email ?? user.Email;
            user.IsEnabled = userDto.IsEnabled ?? user.IsEnabled;
            user.IsAdmin = userDto.IsAdmin ?? user.IsAdmin;

            await _userRepository.UpdateAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsEnabled = user.IsEnabled,
                IsAdmin = user.IsAdmin
            };
        }

        /// <summary>
        /// Remove um usuário do sistema.
        /// </summary>
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user.ToString());
            return true;
        }
    }
}
