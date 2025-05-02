using OrdersProjectDorisAquino.Application.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Application.Interfaces;

namespace OrdersProjectDorisAquino.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrders();
        if (orders == null || !orders.Any())
        {
            return Ok(new List<OrderDto>()); // Retorna array vacío en lugar de null
        }
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        try
        {
            var order = await _orderService.GetOrderById(id);
            return Ok(order);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderId = await _orderService.CreateOrder(orderDto);
            var createdOrder = await _orderService.GetOrderById(orderId);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, createdOrder);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderService.UpdateOrder(id, orderDto);
            if (!result)
                return NotFound();

            var updatedOrder = await _orderService.GetOrderById(id);
            return Ok(updatedOrder);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            var result = await _orderService.DeleteOrder(id);
            return result ? NoContent() : NotFound();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}