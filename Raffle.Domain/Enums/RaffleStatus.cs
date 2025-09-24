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
        PendingAuthorization = 2,
        Authorized = 3,
        Active = 4,
        Closed = 5,
        Drawn = 6,
        Cancelled = 7
    }
}
