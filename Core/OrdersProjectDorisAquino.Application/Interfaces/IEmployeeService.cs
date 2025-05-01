using OrdersProjectDorisAquino.Application.DTOs;

namespace OrdersProjectDorisAquino.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeesDto>> GetAllEmployees();
}
