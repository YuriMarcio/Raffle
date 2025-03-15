using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Application.DTOs.User
{
    public class UserResponseDto
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nome do usuário.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Número de telefone do usuário.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Indica se o email do usuário foi verificado.
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Indica se o número de telefone foi verificado.
        /// </summary>
        public bool IsPhoneVerified { get; set; }

        /// <summary>
        /// Indica se o usuário está ativo.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Data de criação do usuário.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data da última atualização do usuário.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Indica se o usuário tem permissões de administrador.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}

