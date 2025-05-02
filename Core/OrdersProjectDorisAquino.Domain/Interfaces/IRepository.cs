using OrdersProjectDorisAquino.Domain.Entities;

namespace OrdersProjectDorisAquino.Domain.Interfaces;

public interface IRepository
{
    // Métodos para Customers
    Task<IEnumerable<Customer>> GetAllCustomers();

    Task<IEnumerable<Employee>> GetAllEmployees();
    Task<IEnumerable<Product>> GetAllProducts();

    // Métodos para Orders
    Task<IEnumerable<Order>> GetAllOrders();
    Task<Order> GetOrderById(int id);
    Task<int> CreateOrder(Order order);
    Task<bool> UpdateOrder(Order order);
    Task<bool> ProductExists(int productId);
    Task<bool> UpdateFullOrder(Order order);
    Task<bool> DeleteOrder(int id);
    Task<Order> GetOrderByIdWithDetails(int id);
}