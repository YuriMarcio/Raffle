using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Raffle.Domain.Entities;
using Raffle.Domain.Entities.Raffle;
using Raffle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Raffle.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RaffleDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<RaffleDbContext>>()))
            {
                // Check if we already have raffles
                if (context.Raffles.Any())
                {
                    return; // DB has been seeded
                }

                // Create sample organizations (users)
                var organizations = new[]
                {
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Instituto Esperança",
                        Email = "contato@institutoesperanca.org.br",
                        DocumentNumber = "12.345.678/0001-90",
                        IsNonProfit = true,
                        TradeName = "Instituto Esperança"
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "ABC Solidária",
                        Email = "contato@abcsolidaria.org.br",
                        DocumentNumber = "23.456.789/0001-01",
                        IsNonProfit = true,
                        TradeName = "ABC Solidária"
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "ONG Vida Nova",
                        Email = "contato@vidanova.org.br",
                        DocumentNumber = "34.567.890/0001-12",
                        IsNonProfit = true,
                        TradeName = "ONG Vida Nova"
                    }
                };

                context.Users.AddRange(organizations);
                context.SaveChanges();

                // Create sample raffles
                var raffles = new[]
                {
                    new RaffleEntity
                    {
                        Id = "1",
                        Title = "Honda Civic 2024 0km",
                        Description = "Sedan completo com todos os opcionais. Cor prata, automático, com garantia de fábrica.",
                        Type = RaffleType.Car,
                        TicketPrice = 50,
                        NumberOfTickets = 5000,
                        TicketsSold = 3421,
                        Status = RaffleStatus.Active,
                        IsEnable = true,
                        MaxTicketPerUser = 100,
                        MaxParticipants = 5000,
                        PaymentMethod = PaymentMethod.Pix,
                        StartDate = DateTime.UtcNow.AddDays(-30),
                        EndDate = DateTime.UtcNow.AddDays(15),
                        DrawDate = DateTime.UtcNow.AddDays(16),
                        UniqueLink = "civic2024",
                        UserId = organizations[0].Id,
                        SCPCAuthorizationNumber = "SPA/MF-2024-001234",
                        SCPCAuthorizationDate = DateTime.UtcNow.AddDays(-35),
                        SCPCExpirationDate = DateTime.UtcNow.AddDays(30),
                        FundraisingPurpose = "Construção do novo hospital infantil",
                        DrawLocation = "Auditório do Instituto - Transmissão ao vivo",
                        Regulation = "Sorteio promocional autorizado pela SPA/MF conforme Lei 5.768/71",
                        CoverImageUrl = "/images/honda-civic.jpg"
                    },
                    new RaffleEntity
                    {
                        Id = "2",
                        Title = "iPhone 15 Pro Max 1TB",
                        Description = "Smartphone top de linha Apple, 1TB de armazenamento, cor Titanium Natural.",
                        Type = RaffleType.Electronic,
                        TicketPrice = 20,
                        NumberOfTickets = 2000,
                        TicketsSold = 1543,
                        Status = RaffleStatus.Active,
                        IsEnable = true,
                        MaxTicketPerUser = 50,
                        MaxParticipants = 2000,
                        PaymentMethod = PaymentMethod.Pix,
                        StartDate = DateTime.UtcNow.AddDays(-20),
                        EndDate = DateTime.UtcNow.AddDays(10),
                        DrawDate = DateTime.UtcNow.AddDays(11),
                        UniqueLink = "iphone15",
                        UserId = organizations[1].Id,
                        SCPCAuthorizationNumber = "SPA/MF-2024-001235",
                        SCPCAuthorizationDate = DateTime.UtcNow.AddDays(-25),
                        SCPCExpirationDate = DateTime.UtcNow.AddDays(20),
                        FundraisingPurpose = "Programa de alimentação para famílias carentes",
                        DrawLocation = "Live no Instagram @abcsolidaria",
                        Regulation = "Sorteio promocional autorizado pela SPA/MF conforme Lei 5.768/71",
                        CoverImageUrl = "/images/iphone15.jpg"
                    },
                    new RaffleEntity
                    {
                        Id = "3",
                        Title = "Moto Honda CG 160 Titan",
                        Description = "Moto 0km, documentada e emplacada. Cor vermelha, partida elétrica.",
                        Type = RaffleType.Car,
                        TicketPrice = 10,
                        NumberOfTickets = 2000,
                        TicketsSold = 890,
                        Status = RaffleStatus.Active,
                        IsEnable = true,
                        MaxTicketPerUser = 100,
                        MaxParticipants = 2000,
                        PaymentMethod = PaymentMethod.Pix,
                        StartDate = DateTime.UtcNow.AddDays(-15),
                        EndDate = DateTime.UtcNow.AddDays(20),
                        DrawDate = DateTime.UtcNow.AddDays(21),
                        UniqueLink = "cg160",
                        UserId = organizations[2].Id,
                        SCPCAuthorizationNumber = "SPA/MF-2024-001236",
                        SCPCAuthorizationDate = DateTime.UtcNow.AddDays(-20),
                        SCPCExpirationDate = DateTime.UtcNow.AddDays(30),
                        FundraisingPurpose = "Reforma da sede da ONG",
                        DrawLocation = "Sede da ONG - Transmissão pelo YouTube",
                        Regulation = "Sorteio promocional autorizado pela SPA/MF conforme Lei 5.768/71",
                        CoverImageUrl = "/images/honda-cg.jpg"
                    },
                    new RaffleEntity
                    {
                        Id = "4",
                        Title = "PlayStation 5 + 10 Jogos",
                        Description = "Console PlayStation 5 com 10 jogos AAA inclusos. Edição Standard com 2 controles.",
                        Type = RaffleType.Games,
                        TicketPrice = 25,
                        NumberOfTickets = 1000,
                        TicketsSold = 654,
                        Status = RaffleStatus.Active,
                        IsEnable = true,
                        MaxTicketPerUser = 20,
                        MaxParticipants = 1000,
                        PaymentMethod = PaymentMethod.Pix,
                        StartDate = DateTime.UtcNow.AddDays(-10),
                        EndDate = DateTime.UtcNow.AddDays(5),
                        DrawDate = DateTime.UtcNow.AddDays(6),
                        UniqueLink = "ps5bundle",
                        UserId = organizations[0].Id,
                        SCPCAuthorizationNumber = "SPA/MF-2024-001237",
                        SCPCAuthorizationDate = DateTime.UtcNow.AddDays(-15),
                        SCPCExpirationDate = DateTime.UtcNow.AddDays(15),
                        FundraisingPurpose = "Compra de equipamentos médicos",
                        DrawLocation = "Live no Facebook",
                        Regulation = "Sorteio promocional autorizado pela SPA/MF conforme Lei 5.768/71",
                        CoverImageUrl = "/images/ps5.jpg"
                    },
                    new RaffleEntity
                    {
                        Id = "5",
                        Title = "Vale Compras R$ 5.000",
                        Description = "Vale compras no valor de R$ 5.000 para usar em qualquer loja do Shopping Center Norte.",
                        Type = RaffleType.Vouchers,
                        TicketPrice = 15,
                        NumberOfTickets = 1500,
                        TicketsSold = 1200,
                        Status = RaffleStatus.Active,
                        IsEnable = true,
                        MaxTicketPerUser = 30,
                        MaxParticipants = 1500,
                        PaymentMethod = PaymentMethod.Pix,
                        StartDate = DateTime.UtcNow.AddDays(-7),
                        EndDate = DateTime.UtcNow.AddDays(3),
                        DrawDate = DateTime.UtcNow.AddDays(4),
                        UniqueLink = "vale5000",
                        UserId = organizations[1].Id,
                        SCPCAuthorizationNumber = "SPA/MF-2024-001238",
                        SCPCAuthorizationDate = DateTime.UtcNow.AddDays(-10),
                        SCPCExpirationDate = DateTime.UtcNow.AddDays(10),
                        FundraisingPurpose = "Projeto educacional para crianças",
                        DrawLocation = "Sorteio eletrônico certificado",
                        Regulation = "Sorteio promocional autorizado pela SPA/MF conforme Lei 5.768/71",
                        CoverImageUrl = "/images/vale-compras.jpg"
                    },
                    new RaffleEntity
                    {
                        Id = "6",
                        Title = "Smart TV Samsung 65\" 4K",
                        Description = "Smart TV Samsung 65 polegadas, 4K, Crystal UHD, com Alexa integrada.",
                        Type = RaffleType.Electronic,
                        TicketPrice = 30,
                        NumberOfTickets = 800,
                        TicketsSold = 450,
                        Status = RaffleStatus.Active,
                        IsEnable = true,
                        MaxTicketPerUser = 10,
                        MaxParticipants = 800,
                        PaymentMethod = PaymentMethod.Pix,
                        StartDate = DateTime.UtcNow.AddDays(-5),
                        EndDate = DateTime.UtcNow.AddDays(12),
                        DrawDate = DateTime.UtcNow.AddDays(13),
                        UniqueLink = "tv65samsung",
                        UserId = organizations[2].Id,
                        SCPCAuthorizationNumber = "SPA/MF-2024-001239",
                        SCPCAuthorizationDate = DateTime.UtcNow.AddDays(-8),
                        SCPCExpirationDate = DateTime.UtcNow.AddDays(20),
                        FundraisingPurpose = "Manutenção das atividades da ONG",
                        DrawLocation = "Transmissão ao vivo no Instagram",
                        Regulation = "Sorteio promocional autorizado pela SPA/MF conforme Lei 5.768/71",
                        CoverImageUrl = "/images/samsung-tv.jpg"
                    }
                };

                context.Raffles.AddRange(raffles);
                context.SaveChanges();

                // Create prizes for each raffle
                var prizes = new List<Prize>();

                // Prizes for Honda Civic raffle
                prizes.Add(new Prize
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Honda Civic 2024",
                    Description = "Sedan 0km completo",
                    Value = 180000,
                    Quantity = 1,
                    Type = PrizeType.Product,
                    RaffleId = "1"
                });

                // Prizes for iPhone raffle
                prizes.Add(new Prize
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "iPhone 15 Pro Max",
                    Description = "1TB Titanium Natural",
                    Value = 10000,
                    Quantity = 1,
                    Type = PrizeType.Product,
                    RaffleId = "2"
                });

                // Prizes for Moto raffle
                prizes.Add(new Prize
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Honda CG 160 Titan",
                    Description = "Moto 0km",
                    Value = 18000,
                    Quantity = 1,
                    Type = PrizeType.Product,
                    RaffleId = "3"
                });

                // Prizes for PS5 raffle
                prizes.Add(new Prize
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "PlayStation 5 Bundle",
                    Description = "Console + 10 jogos + 2 controles",
                    Value = 5000,
                    Quantity = 1,
                    Type = PrizeType.Product,
                    RaffleId = "4"
                });

                // Prizes for Vale Compras
                prizes.Add(new Prize
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Vale Compras",
                    Description = "R$ 5.000 em compras",
                    Value = 5000,
                    Quantity = 1,
                    Type = PrizeType.Product,
                    RaffleId = "5"
                });

                // Prizes for Smart TV
                prizes.Add(new Prize
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Smart TV Samsung 65\"",
                    Description = "4K Crystal UHD",
                    Value = 4000,
                    Quantity = 1,
                    Type = PrizeType.Product,
                    RaffleId = "6"
                });

                context.Prizes.AddRange(prizes);
                context.SaveChanges();
            }
        }
    }
}