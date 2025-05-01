using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace OrdersProjectDorisAquino.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductsController : Controller
{
    private readonly IProductService service;

    public ProductsController(IProductService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return Ok(await service.GetAllProducts());
    }
}