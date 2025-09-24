using System;
using System.Threading.Tasks;
using Raffle.Domain.Entities;

namespace Raffle.Application.Services
{
    public interface ILegalComplianceService
    {
        Task<bool> ValidateNonProfitOrganization(string cnpj);
        Task<bool> ValidateSCPCAuthorization(string authorizationNumber, DateTime issueDate);
        Task<bool> CheckAuthorizationExpiry(DateTime expirationDate);
        Task<bool> ValidateLegalRepresentative(string cpf, string name);
        Task<LegalComplianceCheckResult> PerformFullComplianceCheck(User user);
    }

    public class LegalComplianceCheckResult
    {
        public bool IsCompliant { get; set; }
        public string Status { get; set; }
        public string[] Issues { get; set; }
        public string[] RequiredDocuments { get; set; }
        public DateTime CheckedAt { get; set; }
    }

    public class LegalComplianceService : ILegalComplianceService
    {
        public async Task<bool> ValidateNonProfitOrganization(string cnpj)
        {
            // Implementação futura: integração com API da Receita Federal
            // Por enquanto, validação simulada baseada em padrões conhecidos de CNPJ de entidades sem fins lucrativos

            await Task.Delay(100); // Simula latência de API

            // Validação básica do CNPJ
            if (string.IsNullOrWhiteSpace(cnpj) || cnpj.Length != 14)
                return false;

            // Simulação: CNPJs que começam com determinados números são frequentemente de entidades sem fins lucrativos
            // Na implementação real, isso seria verificado através da API da Receita Federal
            var firstTwoDigits = cnpj.Substring(0, 2);
            var nonProfitPrefixes = new[] { "04", "05", "06", "07", "08", "09", "10", "11", "12", "13" };

            return Array.Exists(nonProfitPrefixes, prefix => prefix == firstTwoDigits);
        }

        public async Task<bool> ValidateSCPCAuthorization(string authorizationNumber, DateTime issueDate)
        {
            // Implementação futura: integração com API do SCPC/SPA
            // Por enquanto, validação simulada

            await Task.Delay(100);

            if (string.IsNullOrWhiteSpace(authorizationNumber))
                return false;

            // Verifica se a data de emissão não é futura
            if (issueDate > DateTime.UtcNow)
                return false;

            // Verifica formato do número de autorização (exemplo: SPA/2024/12345)
            var pattern = @"^SPA/\d{4}/\d{5}$";
            var regex = new System.Text.RegularExpressions.Regex(pattern);

            return regex.IsMatch(authorizationNumber);
        }

        public async Task<bool> CheckAuthorizationExpiry(DateTime expirationDate)
        {
            await Task.Delay(10);

            // Verifica se a autorização ainda está válida
            return expirationDate > DateTime.UtcNow;
        }

        public async Task<bool> ValidateLegalRepresentative(string cpf, string name)
        {
            // Implementação futura: integração com API da Receita Federal
            // Por enquanto, validação básica

            await Task.Delay(100);

            if (string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(name))
                return false;

            // Validação básica do CPF (11 dígitos)
            if (cpf.Length != 11)
                return false;

            // Verifica se o nome tem pelo menos duas palavras
            var nameParts = name.Trim().Split(' ');
            if (nameParts.Length < 2)
                return false;

            return true;
        }

        public async Task<LegalComplianceCheckResult> PerformFullComplianceCheck(User user)
        {
            var result = new LegalComplianceCheckResult
            {
                CheckedAt = DateTime.UtcNow,
                Issues = new string[0],
                RequiredDocuments = new string[0]
            };

            var issues = new System.Collections.Generic.List<string>();
            var requiredDocs = new System.Collections.Generic.List<string>();

            // Verifica se é CNPJ
            if (user.DocumentType != "CNPJ")
            {
                issues.Add("Apenas organizações com CNPJ podem criar sorteios promocionais");
                result.IsCompliant = false;
                result.Status = "REJECTED";
                result.Issues = issues.ToArray();
                return result;
            }

            // Verifica se é organização sem fins lucrativos
            var isNonProfit = await ValidateNonProfitOrganization(user.DocumentNumber);
            if (!isNonProfit)
            {
                issues.Add("Organização deve ser sem fins lucrativos conforme Lei 5.768/71");
            }

            // Verifica documentos obrigatórios
            if (string.IsNullOrWhiteSpace(user.SocialStatuteUrl))
            {
                requiredDocs.Add("Estatuto Social");
            }

            if (string.IsNullOrWhiteSpace(user.ElectionMinutesUrl))
            {
                requiredDocs.Add("Ata de Eleição da Diretoria");
            }

            // Verifica autorização SCPC
            if (string.IsNullOrWhiteSpace(user.SCPCAuthorizationNumber))
            {
                issues.Add("Autorização SCPC/SPA é obrigatória");
                requiredDocs.Add("Certificado de Autorização SCPC/SPA");
            }
            else if (user.SCPCAuthorizationDate.HasValue)
            {
                var isValidAuth = await ValidateSCPCAuthorization(
                    user.SCPCAuthorizationNumber,
                    user.SCPCAuthorizationDate.Value
                );

                if (!isValidAuth)
                {
                    issues.Add("Número de autorização SCPC/SPA inválido");
                }

                if (user.SCPCExpirationDate.HasValue)
                {
                    var isNotExpired = await CheckAuthorizationExpiry(user.SCPCExpirationDate.Value);
                    if (!isNotExpired)
                    {
                        issues.Add("Autorização SCPC/SPA expirada");
                    }
                }
            }

            // Verifica representante legal
            if (string.IsNullOrWhiteSpace(user.LegalRepresentativeCPF) ||
                string.IsNullOrWhiteSpace(user.LegalRepresentativeName))
            {
                requiredDocs.Add("Identificação do Representante Legal");
            }
            else
            {
                var isValidRep = await ValidateLegalRepresentative(
                    user.LegalRepresentativeCPF,
                    user.LegalRepresentativeName
                );

                if (!isValidRep)
                {
                    issues.Add("Dados do representante legal inválidos");
                }
            }

            // Define o resultado final
            result.Issues = issues.ToArray();
            result.RequiredDocuments = requiredDocs.ToArray();

            if (issues.Count == 0 && requiredDocs.Count == 0)
            {
                result.IsCompliant = true;
                result.Status = "APPROVED";
            }
            else if (requiredDocs.Count > 0)
            {
                result.IsCompliant = false;
                result.Status = "PENDING";
            }
            else
            {
                result.IsCompliant = false;
                result.Status = "REJECTED";
            }

            return result;
        }
    }
}