using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddDbContext<RaffleDbContext>(options =>
               options.UseMySql(
                   configuration.GetConnectionString("MySQLConnection"),
                   ServerVersion.AutoDetect(configuration.GetConnectionString("MySQLConnection"))
               ));

            return services;
        }
    }
}
