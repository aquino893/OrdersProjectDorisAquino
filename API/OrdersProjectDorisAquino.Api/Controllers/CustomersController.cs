using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace OrdersProjectDorisAquino.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomersController : Controller
{
    private readonly ICustomerService service;

    public CustomersController(ICustomerService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        return Ok(await service.GetAllCustomers());
    }
}