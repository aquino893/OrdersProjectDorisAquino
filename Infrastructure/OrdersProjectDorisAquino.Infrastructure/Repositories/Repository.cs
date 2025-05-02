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
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateFullOrder(Order order)
    {
        // Obtener los IDs de los productos existentes
        var existingDetailIds = await context.OrderDetails
            .Where(od => od.OrderID == order.OrderID)
            .Select(od => od.ProductID)
            .ToListAsync();

        // Eliminar detalles que ya no están en la orden actualizada
        var detailsToRemove = context.OrderDetails
            .Where(od => od.OrderID == order.OrderID && 
                        !order.OrderDetails.Any(d => d.ProductID == od.ProductID));
        context.OrderDetails.RemoveRange(detailsToRemove);

        // Actualizar o agregar detalles
        foreach (var detail in order.OrderDetails)
        {
            var existingDetail = await context.OrderDetails
                .FirstOrDefaultAsync(od => 
                    od.OrderID == order.OrderID && 
                    od.ProductID == detail.ProductID);

            if (existingDetail != null)
            {
                // Actualizar detalle existente
                existingDetail.UnitPrice = detail.UnitPrice;
                existingDetail.Quantity = detail.Quantity;
                existingDetail.Discount = detail.Discount;
                context.Entry(existingDetail).State = EntityState.Modified;
            }
            else
            {
                // Agregar nuevo detalle
                context.OrderDetails.Add(new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = detail.ProductID,
                    UnitPrice = detail.UnitPrice,
                    Quantity = detail.Quantity,
                    Discount = detail.Discount
                });
            }
        }

        // Marcar la orden como modificada
        context.Entry(order).State = EntityState.Modified;

        // Guardar todos los cambios
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ProductExists(int productId)
    {
        return await context.Products
            .AnyAsync(p => p.ProductID == productId);
    }

    public async Task<Order> GetOrderByIdWithDetails(int id)
    {
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.OrderID == id);
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
