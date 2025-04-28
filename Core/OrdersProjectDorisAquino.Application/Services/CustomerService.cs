using OrdersProjectDorisAquino.Application.CustomExceptions;
using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Mapping;
using OrdersProjectDorisAquino.Domain.Entities;
using OrdersProjectDorisAquino.Domain.Interfaces;

namespace OrdersProjectDorisAquino.Application.Services;
public class CustomerService(IRepository repository) : ICustomerService
{

    public async Task<IEnumerable<CustomersDto>> GetAllCustomers()
    {
        var customersDb = await repository.GetAllCustomers().ConfigureAwait(false);
        var customerDtos = customersDb.Select(b => b.ToCustomersDto()).ToList();
        return customerDtos;
    }

}
