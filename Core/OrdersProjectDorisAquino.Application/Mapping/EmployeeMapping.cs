using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Domain.Entities;

namespace OrdersProjectDorisAquino.Application.Mapping;

public static class EmployeeMapping
{
    public static EmployeesDto ToEmployeesDto(this Employee employee)
    {
        var employeeDto = new EmployeesDto
        {
            EmployeeId = employee.EmployeeID,
            FullName = $"{employee.FirstName} {employee.LastName}"
        };
        return employeeDto;
    }
}
