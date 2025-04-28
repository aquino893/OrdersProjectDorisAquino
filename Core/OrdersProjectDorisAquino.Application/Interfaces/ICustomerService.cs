using OrdersProjectDorisAquino.Application.DTOs;

namespace OrdersProjectDorisAquino.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomersDto>> GetAllCustomers();
}
