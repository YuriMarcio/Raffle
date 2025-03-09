using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Aplication.DTOs.AutenticationDto
{
    public class UserLoginDtoRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
