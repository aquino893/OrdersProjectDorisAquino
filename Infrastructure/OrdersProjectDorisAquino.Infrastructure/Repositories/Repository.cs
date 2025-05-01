using OrdersProjectDorisAquino.Domain.Entities;
using OrdersProjectDorisAquino.Domain.Interfaces;
using OrdersProjectDorisAquino.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace OrdersProjectDorisAquino.Infrastructure.Repositories;

public class Repository(AppDbContext context) : IRepository
{
    public async Task<IEnumerable<Customer>> GetAllCustomers()
    {
        return await context.Customers
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        return await context.Employees
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);
    }


}
