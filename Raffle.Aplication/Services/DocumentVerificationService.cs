using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Raffle.Domain.Entities;
using System.Text.RegularExpressions;

namespace Raffle.Aplication.Services
{
    public interface IDocumentVerificationService
    {
        Task<DocumentVerificationResult> VerifyCPF(string cpf, string name);
        Task<DocumentVerificationResult> VerifyCNPJ(string cnpj, string companyName);
        bool ValidateCPF(string cpf);
        bool ValidateCNPJ(string cnpj);
    }

    public class DocumentVerificationResult
    {
        public bool IsValid { get; set; }
        public string Status { get; set; } // REGULAR, IRREGULAR, PENDING
        public string Message { get; set; }
        public DateTime VerifiedAt { get; set; }
        public object AdditionalData { get; set; }
    }

    public class DocumentVerificationService : IDocumentVerificationService
    {
        private readonly HttpClient _httpClient;

        public DocumentVerificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DocumentVerificationResult> VerifyCPF(string cpf, string name)
        {
            // Remove formatação
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            if (!ValidateCPF(cpf))
            {
                return new DocumentVerificationResult
                {
                    IsValid = false,
                    Status = "INVALID",
                    Message = "CPF inválido",
                    VerifiedAt = DateTime.UtcNow
                };
            }

            try
            {
                // Aqui você integraria com APIs reais de verificação
                // Por exemplo: Receita Federal, Serasa, etc.
                // Por enquanto, vamos simular uma verificação

                await Task.Delay(1000); // Simula tempo de verificação

                // Simulação: CPFs que começam com "000" são considerados irregulares
                if (cpf.StartsWith("000"))
                {
                    return new DocumentVerificationResult
                    {
                        IsValid = true,
                        Status = "IRREGULAR",
                        Message = "CPF válido mas com pendências",
                        VerifiedAt = DateTime.UtcNow
                    };
                }

                return new DocumentVerificationResult
                {
                    IsValid = true,
                    Status = "REGULAR",
                    Message = "CPF válido e regular",
                    VerifiedAt = DateTime.UtcNow,
                    AdditionalData = new { Name = name }
                };
            }
            catch (Exception ex)
            {
                return new DocumentVerificationResult
                {
                    IsValid = false,
                    Status = "ERROR",
                    Message = $"Erro na verificação: {ex.Message}",
                    VerifiedAt = DateTime.UtcNow
                };
            }
        }

        public async Task<DocumentVerificationResult> VerifyCNPJ(string cnpj, string companyName)
        {
            // Remove formatação
            cnpj = Regex.Replace(cnpj, @"[^\d]", "");

            if (!ValidateCNPJ(cnpj))
            {
                return new DocumentVerificationResult
                {
                    IsValid = false,
                    Status = "INVALID",
                    Message = "CNPJ inválido",
                    VerifiedAt = DateTime.UtcNow
                };
            }

            try
            {
                // Aqui você integraria com APIs reais
                // Por exemplo: Receita Federal, Sintegra, etc.
                await Task.Delay(1500); // Simula tempo de verificação

                // Simulação: CNPJs que começam com "00" são considerados irregulares
                if (cnpj.StartsWith("00"))
                {
                    return new DocumentVerificationResult
                    {
                        IsValid = true,
                        Status = "IRREGULAR",
                        Message = "CNPJ válido mas com pendências fiscais",
                        VerifiedAt = DateTime.UtcNow
                    };
                }

                return new DocumentVerificationResult
                {
                    IsValid = true,
                    Status = "REGULAR",
                    Message = "CNPJ válido e regular",
                    VerifiedAt = DateTime.UtcNow,
                    AdditionalData = new
                    {
                        CompanyName = companyName,
                        Status = "ATIVA",
                        OpenDate = DateTime.UtcNow.AddYears(-2)
                    }
                };
            }
            catch (Exception ex)
            {
                return new DocumentVerificationResult
                {
                    IsValid = false,
                    Status = "ERROR",
                    Message = $"Erro na verificação: {ex.Message}",
                    VerifiedAt = DateTime.UtcNow
                };
            }
        }

        public bool ValidateCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais
            if (new string(cpf[0], 11) == cpf)
                return false;

            // Validação do primeiro dígito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * (10 - i);

            int remainder = sum % 11;
            int firstDigit = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cpf[9].ToString()) != firstDigit)
                return false;

            // Validação do segundo dígito verificador
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            int secondDigit = remainder < 2 ? 0 : 11 - remainder;

            return int.Parse(cpf[10].ToString()) == secondDigit;
        }

        public bool ValidateCNPJ(string cnpj)
        {
            cnpj = Regex.Replace(cnpj, @"[^\d]", "");

            if (cnpj.Length != 14)
                return false;

            // Verifica se todos os dígitos são iguais
            if (new string(cnpj[0], 14) == cnpj)
                return false;

            // Validação do primeiro dígito verificador
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cnpj[12].ToString()) != primeiroDigito)
                return false;

            // Validação do segundo dígito verificador
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cnpj[13].ToString()) == segundoDigito;
        }
    }
}