using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Domain.Entities;

namespace OrdersProjectDorisAquino.Application.Mapping;

public static class OrderMapping
{
    public static OrderDto ToOrderDto(this Order order)
    {
        return new OrderDto
        {
            OrderId = order.OrderID,
            CustomerId = order.CustomerID ?? string.Empty,
            EmployeeId = order.EmployeeID,
            OrderDate = order.OrderDate,
            ShippedDate = order.ShippedDate,
            Freight = decimal.Round(order.Freight, 2),
            ShipName = order.ShipName ?? string.Empty,
            ShipAddress = order.ShipAddress ?? string.Empty,
            ShipCity = order.ShipCity ?? string.Empty,
            ShipRegion = order.ShipRegion,
            ShipPostalCode = order.ShipPostalCode,
            ShipCountry = order.ShipCountry ?? string.Empty,
            OrderDetails = order.OrderDetails?
                .Where(od => od != null)
                .Select(od => od.ToOrderDetailDto())
                .ToList() ?? new List<OrderDetailDto>()
        };
    }

    public static OrderDetailDto ToOrderDetailDto(this OrderDetail orderDetail)
    {
        if (orderDetail == null)
            return new OrderDetailDto();
            
        return new OrderDetailDto
        {
            ProductId = orderDetail.ProductID,
            ProductName = orderDetail.Product?.ProductName ?? string.Empty,
            UnitPrice = orderDetail.UnitPrice,
            Quantity = orderDetail.Quantity,
            Discount = orderDetail.Discount
        };
    }
}