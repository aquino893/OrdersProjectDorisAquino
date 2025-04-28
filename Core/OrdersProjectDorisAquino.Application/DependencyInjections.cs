using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace OrdersProjectDorisAquino.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
}