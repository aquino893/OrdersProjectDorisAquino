using OrdersProjectDorisAquino.Domain.Interfaces;
using OrdersProjectDorisAquino.Infrastructure.Persistence;
using OrdersProjectDorisAquino.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersProjectDorisAquino.Domain.Interfaces.ExternalServices;
using OrdersProjectDorisAquino.Infrastructure.ExternalServices;
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

        services.AddHttpClient<IAddressValidationService, GoogleAddressValidationService>(client =>
        {
            client.BaseAddress = new Uri("https://addressvalidation.googleapis.com"); // Sin barra final
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });
        
        return services;
    }
}
