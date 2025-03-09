using LeadSoft.Common.GlobalDomain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Raffle.Domain.Entities
{
    public class User : CollectionsBase
    {
        // Propriedades herdadas de CollectionsBase
        /// <summary>
        /// Identificador único do usuário no banco de dados.
        /// </summary>
        [Key]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o usuário está habilitado ou desabilitado no sistema.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Data e hora em que o usuário foi criado no sistema.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data e hora da última atualização do usuário no sistema (opcional).
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Propriedades específicas da entidade User com validações
        /// <summary>
        /// Nome do usuário.
        /// </summary>
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Name { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
        [MaxLength(200, ErrorMessage = "O e-mail não pode exceder 200 caracteres.")]
        public string Email { get; set; }

        /// <summary>
        /// Indica se o usuário tem permissões de administrador.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Password { get; set; }
    }
}
