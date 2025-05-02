using OrdersProjectDorisAquino.Application.CustomExceptions;
using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Mapping;
using OrdersProjectDorisAquino.Domain.Entities;
using System.Diagnostics;
using OrdersProjectDorisAquino.Domain.Interfaces;

namespace OrdersProjectDorisAquino.Application.Services;

public class OrderService(IRepository repository) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetAllOrders()
    {
        try
        {
            var orders = await repository.GetAllOrders();
            return orders
                .Where(o => o != null)
                .Select(o => o.ToOrderDto())
                .ToList();
        }
        catch (Exception ex)
        {
            // Log del error
            Debug.WriteLine($"Error retrieving orders: {ex.Message}");
            return Enumerable.Empty<OrderDto>();
        }
    }

    public async Task<OrderDto> GetOrderById(int id)
    {
        var order = await repository.GetOrderById(id);
        if (order == null)
            throw new NotFoundException($"Order with ID {id} not found");
        
        return order.ToOrderDto();
    }

    public async Task<int> CreateOrder(OrderDto orderDto)
    {
        if (orderDto.OrderDate == default)
        {
            throw new ArgumentException("OrderDate is required");
        }
        var order = new Order
        {
            CustomerID = orderDto.CustomerId,
            EmployeeID = orderDto.EmployeeId,
            OrderDate = orderDto.OrderDate,

            ShippedDate = orderDto.ShippedDate,
            Freight = orderDto.Freight,
            ShipName = orderDto.ShipName,
            ShipAddress = orderDto.ShipAddress,
            ShipCity = orderDto.ShipCity,
            ShipRegion = orderDto.ShipRegion ?? string.Empty,
            ShipPostalCode = orderDto.ShipPostalCode ?? string.Empty,
            ShipCountry = orderDto.ShipCountry,
            OrderDetails = (orderDto.OrderDetails ?? Enumerable.Empty<OrderDetailDto>()).Select(od => new OrderDetail
            {
                ProductID = od.ProductId,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList()
        };

        return await repository.CreateOrder(order);
    }

    public async Task<bool> UpdateOrder(int id, OrderDto orderDto)
    {
        if (id != orderDto.OrderId)
            throw new BadRequestException("ID mismatch");

        var existingOrder = await repository.GetOrderById(id);
        if (existingOrder == null)
            throw new NotFoundException($"Order with ID {id} not found");

        // Actualizar propiedades
        existingOrder.CustomerID = orderDto.CustomerId;
        existingOrder.EmployeeID = orderDto.EmployeeId;
        // ... otras propiedades

        return await repository.UpdateOrder(existingOrder);
    }

    public async Task<bool> DeleteOrder(int id)
    {
        var order = await repository.GetOrderById(id);
        if (order == null)
            throw new NotFoundException($"Order with ID {id} not found");

        return await repository.DeleteOrder(id);
    }
}