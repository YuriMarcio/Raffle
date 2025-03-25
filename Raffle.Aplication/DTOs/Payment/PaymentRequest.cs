using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace Raffle.Aplication.DTOs.Payment
{
    public class PaymentRequest
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
