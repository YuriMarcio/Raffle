using Raffle.Aplication.DTOs.AutenticationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(ResgisterDtoRequest aDtoRequest);
        Task<string> LoginAsync(string email, string password);
    }
}