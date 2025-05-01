using OrdersProjectDorisAquino.Domain.Entities;

namespace OrdersProjectDorisAquino.Domain.Interfaces;

public interface IRepository
{
    // Métodos para Customers
    Task<IEnumerable<Customer>> GetAllCustomers();

    Task<IEnumerable<Employee>> GetAllEmployees();
}