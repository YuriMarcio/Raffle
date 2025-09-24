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
        public string? Phone { get; set; }

        /// <summary>
        /// Indica se o número de telefone do cliente foi verificado.
        /// </summary>
        public bool IsPhoneVerified { get; set; }

        /// <summary>
        /// Lista de tickets associados a este cliente.
        /// </summary>
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        /// <summary>
        /// Lista de associações entre a empresa e os sorteios promocionais.
        /// </summary>
        public ICollection<RaffleClient> RaffleClients { get; set; } = new List<RaffleClient>();


        /// <summary>
        /// Indica se o usuário tem permissões de administrador.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Tipo de documento da empresa (sempre CNPJ - conforme Lei 5.768/71)
        /// </summary>
        public string DocumentType { get; set; } = "CNPJ"; // Exclusivamente "CNPJ"

        /// <summary>
        /// Número do CNPJ da empresa (obrigatório)
        /// </summary>
        [MaxLength(18, ErrorMessage = "O documento não pode exceder 18 caracteres.")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Razão social da empresa (obrigatório)
        /// </summary>
        [MaxLength(200, ErrorMessage = "O nome da empresa não pode exceder 200 caracteres.")]
        public string? CompanyName { get; set; }

        /// <summary>
        /// Nome fantasia da empresa (opcional)
        /// </summary>
        [MaxLength(200, ErrorMessage = "O nome fantasia não pode exceder 200 caracteres.")]
        public string? TradeName { get; set; }

        /// <summary>
        /// Status da verificação do documento
        /// </summary>
        public string DocumentVerificationStatus { get; set; } = "PENDING"; // PENDING, VERIFIED, REJECTED

        /// <summary>
        /// Data da verificação do documento
        /// </summary>
        public DateTime? DocumentVerifiedAt { get; set; }

        /// <summary>
        /// Observações sobre a verificação do documento
        /// </summary>
        public string? DocumentVerificationNotes { get; set; }

        /// <summary>
        /// Indica se a empresa pode criar sorteios promocionais (dependente de autorização SCPC)
        /// </summary>
        public bool CanCreateRaffles { get; set; }

        /// <summary>
        /// Limite de sorteios promocionais que a empresa pode criar
        /// </summary>
        public int? RaffleCreationLimit { get; set; }

        /// <summary>
        /// Endereço completo
        /// </summary>
        [MaxLength(500, ErrorMessage = "O endereço não pode exceder 500 caracteres.")]
        public string? Address { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        [MaxLength(9, ErrorMessage = "O CEP não pode exceder 9 caracteres.")]
        public string? PostalCode { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [MaxLength(100, ErrorMessage = "A cidade não pode exceder 100 caracteres.")]
        public string? City { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [MaxLength(2, ErrorMessage = "O estado deve ter 2 caracteres.")]
        public string? State { get; set; }

        // Campos de Conformidade Legal (Lei 5.768/71)

        /// <summary>
        /// Natureza Jurídica da empresa (obrigatório)
        /// </summary>
        [MaxLength(100, ErrorMessage = "A natureza jurídica não pode exceder 100 caracteres.")]
        public string? LegalNature { get; set; }

        /// <summary>
        /// Indica se é uma organização sem fins lucrativos
        /// </summary>
        public bool IsNonProfit { get; set; }

        /// <summary>
        /// Número de autorização SCPC (obrigatório para criar sorteios - Lei 5.768/71)
        /// </summary>
        [MaxLength(50, ErrorMessage = "O número de autorização não pode exceder 50 caracteres.")]
        public string? SCPCAuthorizationNumber { get; set; }

        /// <summary>
        /// Data de emissão da autorização SCPC
        /// </summary>
        public DateTime? SCPCAuthorizationDate { get; set; }

        /// <summary>
        /// Data de validade da autorização SCPC
        /// </summary>
        public DateTime? SCPCExpirationDate { get; set; }

        /// <summary>
        /// URL do estatuto social (PDF)
        /// </summary>
        [MaxLength(500, ErrorMessage = "A URL do estatuto não pode exceder 500 caracteres.")]
        public string? SocialStatuteUrl { get; set; }

        /// <summary>
        /// URL da ata de eleição (PDF)
        /// </summary>
        [MaxLength(500, ErrorMessage = "A URL da ata não pode exceder 500 caracteres.")]
        public string? ElectionMinutesUrl { get; set; }

        /// <summary>
        /// Nome do responsável legal
        /// </summary>
        [MaxLength(200, ErrorMessage = "O nome do responsável não pode exceder 200 caracteres.")]
        public string? LegalRepresentativeName { get; set; }

        /// <summary>
        /// CPF do responsável legal (apenas para identificação - não autoriza criação de sorteios)
        /// </summary>
        [MaxLength(14, ErrorMessage = "O CPF do responsável não pode exceder 14 caracteres.")]
        public string? LegalRepresentativeCPF { get; set; }

        /// <summary>
        /// Status de conformidade legal
        /// </summary>
        public string LegalComplianceStatus { get; set; } = "PENDING"; // PENDING, APPROVED, REJECTED, SUSPENDED

        /// <summary>
        /// Data da última verificação de conformidade
        /// </summary>
        public DateTime? LegalComplianceVerifiedAt { get; set; }

        /// <summary>
        /// Observações sobre conformidade legal
        /// </summary>
        public string? LegalComplianceNotes { get; set; }
    }
}
