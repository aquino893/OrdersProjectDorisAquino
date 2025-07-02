using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using OrdersProjectDorisAquino.Domain.Interfaces.ExternalServices;

namespace OrdersProjectDorisAquino.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}