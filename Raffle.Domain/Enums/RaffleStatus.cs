using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Domain.Enums
{
    public enum RaffleStatus
    {
        Draft = 1,       
        Active = 2,      
        Finished = 3,    
        Cancelled = 4    
    }
}
