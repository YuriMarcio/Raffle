using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Domain.Enums
{
    public enum PaymentMethod
    {
        CreditCard = 1,
        DebitCard = 2,
        BankSlip = 3, // Boleto Bancário
        Pix = 4,
        PayPal = 5,
        ApplePay = 6,
        GooglePay = 7,
        Cash = 8 // Dinheiro
    }
}
