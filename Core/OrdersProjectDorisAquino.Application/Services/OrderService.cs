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
            throw new ArgumentException("ID mismatch");

        var existingOrder = await repository.GetOrderByIdWithDetails(id);
        if (existingOrder == null)
            throw new NotFoundException($"Order with ID {id} not found");

        // Actualizar propiedades principales
        existingOrder.CustomerID = orderDto.CustomerId;
        existingOrder.EmployeeID = orderDto.EmployeeId;
        existingOrder.OrderDate = orderDto.OrderDate;
        existingOrder.Freight = orderDto.Freight;
        existingOrder.ShipName = orderDto.ShipName ?? string.Empty;
        existingOrder.ShipAddress = orderDto.ShipAddress ?? string.Empty;
        existingOrder.ShipCity = orderDto.ShipCity ?? string.Empty;
        existingOrder.ShipPostalCode = orderDto.ShipPostalCode ?? string.Empty;
        existingOrder.ShipCountry = orderDto.ShipCountry ?? string.Empty;

        // Manejo de OrderDetails
        await UpdateOrderDetails(existingOrder, orderDto.OrderDetails);

        return await repository.UpdateOrder(existingOrder);
    }

    private async Task UpdateOrderDetails(Order existingOrder, List<OrderDetailDto> newDetails)
    {
        // Convertir a diccionario para búsqueda rápida
        var newDetailsDict = newDetails.ToDictionary(d => d.ProductId);

        // 1. Eliminar detalles que no están en el DTO
        var detailsToRemove = existingOrder.OrderDetails
            .Where(od => !newDetailsDict.ContainsKey(od.ProductID))
            .ToList();

        foreach (var detail in detailsToRemove)
        {
            existingOrder.OrderDetails.Remove(detail);
        }

        // 2. Actualizar/agregar detalles
        foreach (var detailDto in newDetails)
        {
            var existingDetail = existingOrder.OrderDetails
                .FirstOrDefault(od => od.ProductID == detailDto.ProductId);

            if (existingDetail != null)
            {
                // Actualizar detalle existente
                existingDetail.UnitPrice = detailDto.UnitPrice;
                existingDetail.Quantity = detailDto.Quantity;
                existingDetail.Discount = detailDto.Discount;
            }
            else
            {
                // Verificar que el producto existe
                var productExists = await repository.ProductExists(detailDto.ProductId);
                if (!productExists)
                    throw new ArgumentException($"Product with ID {detailDto.ProductId} not found");

                // Agregar nuevo detalle
                existingOrder.OrderDetails.Add(new OrderDetail
                {
                    ProductID = detailDto.ProductId,
                    UnitPrice = detailDto.UnitPrice,
                    Quantity = detailDto.Quantity,
                    Discount = detailDto.Discount
                });
            }
        }
    }

    public async Task<bool> DeleteOrder(int id)
    {
        var order = await repository.GetOrderById(id);
        if (order == null)
            throw new NotFoundException($"Order with ID {id} not found");

        return await repository.DeleteOrder(id);
    }
}