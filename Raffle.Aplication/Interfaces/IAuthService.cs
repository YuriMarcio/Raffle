using Raffle.Aplication.DTOs.AutenticationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task<string> RegisterAsync(string name, string email, string password, string phone);
        // Task<string> RegisterCPFAsync(RegisterCPFRequest request); // REMOVIDO - Lei 5.768/71 proíbe CPF
        Task<string> RegisterCNPJAsync(RegisterCNPJRequest request);
        Task<object> GetDocumentVerificationStatus(string userId);
    }
}