using LeadSoft.Common.GlobalDomain.Entities;
using Raffle.Domain.Entities.Tickets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Raffle.Domain.Entities
{
    public class User
    {
        // Propriedades herdadas de CollectionsBase
        /// <summary>
        /// Identificador único do cliente no banco de dados.
        /// </summary>
        [Key]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o cliente está habilitado ou desabilitado no sistema.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Data e hora em que o cliente foi criado no sistema.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data e hora da última atualização do cliente no sistema (opcional).
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Propriedades específicas da entidade Client com validações
        /// <summary>
        /// Nome do cliente.
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Name { get; set; }

        /// <summary>
        /// Endereço de e-mail do cliente.
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        [MaxLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        public string Email { get; set; }

        /// <summary>
        /// Indica se o email do cliente foi verificado.
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Senha do cliente para autenticação no sistema.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Password { get; set; }

        /// <summary>
        /// Número de telefone do cliente.
        /// </summary>
        [Phone(ErrorMessage = "O número de telefone informado não é válido.")]
        [MaxLength(15, ErrorMessage = "O número de telefone não pode exceder 15 caracteres.")]
        public string Phone { get; set; }

        /// <summary>
        /// Indica se o número de telefone do cliente foi verificado.
        /// </summary>
        public bool IsPhoneVerified { get; set; }

        /// <summary>
        /// Lista de tickets associados a este cliente.
        /// </summary>
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        /// <summary>
        /// Lista de associações entre o cliente e as rifas.
        /// </summary>
        public ICollection<RaffleClient> RaffleClients { get; set; } = new List<RaffleClient>();


        /// <summary>
        /// Indica se o usuário tem permissões de administrador.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
