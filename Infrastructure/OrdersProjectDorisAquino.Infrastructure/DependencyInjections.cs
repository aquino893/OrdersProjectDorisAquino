using OrdersProjectDorisAquino.Domain.Interfaces;
using OrdersProjectDorisAquino.Infrastructure.Persistence;
using OrdersProjectDorisAquino.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OrdersProjectDorisAquino.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                               IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("OrdersDbConnection"),
                opt => opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
        });
        
        services.AddTransient<IRepository, Repository>();
        
        return services;
    }
}
