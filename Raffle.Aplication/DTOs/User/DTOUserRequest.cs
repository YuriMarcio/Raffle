using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Application.DTOs.User
{
    public class CreateUserRequestDto
    {
        /// <summary>
        /// Nome do usuário.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Número de telefone do usuário (opcional).
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Indica se o usuário tem permissões de administrador.
        /// </summary>
        public bool IsAdmin { get; set; }
    }

    public class UpdateUserRequestDto
    {
        /// <summary>
        /// Nome do usuário (opcional).
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário (opcional).
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Número de telefone do usuário (opcional).
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Indica se o usuário está habilitado.
        /// </summary>
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// Indica se o usuário tem permissões de administrador.
        /// </summary>
        public bool? IsAdmin { get; set; }
    }
}

