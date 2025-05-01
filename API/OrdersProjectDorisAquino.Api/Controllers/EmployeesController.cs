using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace OrdersProjectDorisAquino.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EmployeesController : Controller
{
    private readonly IEmployeeService service;

    public EmployeesController(IEmployeeService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        return Ok(await service.GetAllEmployees());
    }
}