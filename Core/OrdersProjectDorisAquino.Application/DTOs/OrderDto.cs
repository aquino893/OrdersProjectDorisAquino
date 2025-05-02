namespace OrdersProjectDorisAquino.Application.DTOs;

public class OrderDto
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; } 
    public decimal Freight { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public string? ShipRegion { get; set; }
    public string? ShipPostalCode { get; set; }
    public string ShipCountry { get; set; } = string.Empty;
    public List<OrderDetailDto> OrderDetails { get; set; } = new();
}