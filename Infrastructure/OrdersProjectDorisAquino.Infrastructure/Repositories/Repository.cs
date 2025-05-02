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

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await context.Products
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .Where(o => o.OrderDate != null) // Filtra órdenes válidas
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<Order> GetOrderById(int id)
    {
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.OrderID == id)
            .ConfigureAwait(false);
    }

    public async Task<int> CreateOrder(Order order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync().ConfigureAwait(false);
        return order.OrderID;
    }

    public async Task<bool> UpdateOrder(Order order)
    {
        context.Orders.Update(order);
        return await context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool> DeleteOrder(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order == null)
            return false;

        context.Orders.Remove(order);
        return await context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }


}
