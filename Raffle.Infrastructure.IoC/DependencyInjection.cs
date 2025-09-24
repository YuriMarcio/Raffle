using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Raffle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MySQLConnection") ??
                "Server=sortio-mysql;Database=sortio_db;User=sortio_user;Password=sortio_pass_2024;Port=3306";

            services.AddDbContext<RaffleDbContext>(options =>
               options.UseMySql(
                   connectionString,
                   new MySqlServerVersion(new Version(8, 0, 21))
               ));

            return services;
        }
    }
}
