using OrdersProjectDorisAquino.Application.DTOs;

namespace OrdersProjectDorisAquino.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllOrders();
    Task<OrderDto> GetOrderById(int id);
    Task<int> CreateOrder(OrderDto orderDto);
    Task<bool> UpdateOrder(int id, OrderDto orderDto);
    Task<bool> DeleteOrder(int id);
}