using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Domain.Entities;

namespace OrdersProjectDorisAquino.Application.Mapping;

public static class CustomerMapping
{
    public static CustomersDto ToCustomersDto(this Customer customer)
    {
        var customerDto = new CustomersDto
        {
            CustomerId = customer.CustomerId,
            CompanyName = customer.CompanyName,
            ContactName = customer.ContactName,
            ContactTitle = customer.ContactTitle
        };
        return customerDto;
    }
}
