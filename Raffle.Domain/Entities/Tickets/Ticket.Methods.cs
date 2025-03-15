using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raffle.Domain.Entities.Tickets
{
    /// <summary>
    /// Ticket entity methods

    /// </summary>
    public partial class Ticket
    {

        public Ticket() { }


        /// <summary>
        /// Constructor based on Raffle and Client
        /// </summary>
        public Ticket(string raffleId, Guid? clientId)
        {
            RaffleId = raffleId;
        }

        /// <summary>
        /// Method sets Ticket value
        /// </summary>
        /// <param name="value">The ticket value</param>
        /// <returns>Self for chain call.</returns>
        public Ticket SetValue(int value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        /// Method sets Ticket as winner
        /// </summary>
        /// <returns>Self for chain call.</returns>
        public Ticket SetAsWinner()
        {
            IsWinner = true;
            return this;
        }

        /// <summary>
        /// Method sets Ticket purchase date
        /// </summary>
        /// <param name="purchaseDate">The purchase date</param>
        /// <returns>Self for chain call.</returns>
        public Ticket SetPurchaseDate(DateTime purchaseDate)
        {
            PurchaseDate = purchaseDate;
            return this;
        }

        /// <summary>
        /// Method generates and saves tickets in bulk
        /// </summary>
        /// <param name="raffleId">The raffle ID</param>
        /// <param name="quantity">The number of tickets to generate</param>
        /// <returns>List of generated tickets</returns>
        public static List<Ticket> GenerateTickets(string raffleId, int quantity)
        {
            // Gera uma lista de números únicos, por exemplo, de 1 até quantity
            var ticketNumbers = Enumerable.Range(1, quantity).ToList();

            // Cria a lista de tickets com a quantidade especificada
            var tickets = new List<Ticket>(quantity);

            // Itera sobre os números únicos gerados
            foreach (var number in ticketNumbers)
            {
                tickets.Add(new Ticket
                {
                    Id = Guid.NewGuid().ToString(), // Gera um Id único para cada ticket
                    RaffleId = raffleId,
                    UserId = null, // Inicialmente sem cliente
                    Value = number,  // Utiliza o número gerado como valor do ticket
                    IsWinner = false, // Inicialmente, nenhum ticket é vencedor
                });
            }

            return tickets;
        }

        /// <summary>
        /// Method checks if the ticket is a winner
        /// </summary>
        /// <returns>True if the ticket is a winner, otherwise false</returns>
        public bool IsAWinner()
        {
            return IsWinner;
        }

        //public DateTime OrderPurchase(string aClientId,DateTime aPurchaseDate, string aTicketId)
        //{

        //}

    }
}