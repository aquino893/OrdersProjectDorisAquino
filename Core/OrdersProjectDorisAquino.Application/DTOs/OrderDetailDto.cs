namespace OrdersProjectDorisAquino.Application.DTOs;

public class OrderDetailDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; } = 1;
    public float Discount { get; set; }
    public decimal Total => decimal.Round(UnitPrice * Quantity * (1 - (decimal)Discount), 2);
}