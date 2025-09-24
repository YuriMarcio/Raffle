using System.ComponentModel.DataAnnotations;

namespace Raffle.Aplication.DTOs.AutenticationDto
{
    /// <summary>
    /// DESABILITADO - Lei 5.768/71: Apenas empresas (CNPJ) podem criar sorteios promocionais.
    /// Pessoas físicas (CPF) NÃO podem realizar sorteios promocionais.
    /// </summary>
    [Obsolete("Uso de CPF para sorteios promocionais é proibido pela Lei 5.768/71. Use apenas CNPJ.", true)]
    public class RegisterCPFRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$",
            ErrorMessage = "CPF inválido. Use o formato 000.000.000-00 ou apenas números.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "A senha deve conter letras maiúsculas, minúsculas, números e caracteres especiais.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Telefone inválido.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [RegularExpression(@"^\d{5}-\d{3}$|^\d{8}$",
            ErrorMessage = "CEP inválido. Use o formato 00000-000 ou apenas números.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [MaxLength(100)]
        public string City { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Use a sigla do estado com 2 letras.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Você deve aceitar os termos de uso.")]
        public bool AcceptTerms { get; set; }
    }
}