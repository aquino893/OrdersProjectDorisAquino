using OrdersProjectDorisAquino.Application.DTOs;

namespace OrdersProjectDorisAquino.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductsDto>> GetAllProducts();
}
