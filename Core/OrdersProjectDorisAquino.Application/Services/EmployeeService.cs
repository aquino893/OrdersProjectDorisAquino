using OrdersProjectDorisAquino.Application.CustomExceptions;
using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Mapping;
using OrdersProjectDorisAquino.Domain.Entities;
using OrdersProjectDorisAquino.Domain.Interfaces;

namespace OrdersProjectDorisAquino.Application.Services;
public class EmployeeService(IRepository repository) : IEmployeeService
{

    public async Task<IEnumerable<EmployeesDto>> GetAllEmployees()
    {
        var employeesDb = await repository.GetAllEmployees().ConfigureAwait(false);
        var employeeDtos = employeesDb.Select(b => b.ToEmployeesDto()).ToList();
        return employeeDtos;
    }

}
