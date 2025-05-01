using OrdersProjectDorisAquino.Application.CustomExceptions;
using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Application.Interfaces;
using OrdersProjectDorisAquino.Application.Mapping;
using OrdersProjectDorisAquino.Domain.Entities;
using OrdersProjectDorisAquino.Domain.Interfaces;

namespace OrdersProjectDorisAquino.Application.Services;
public class ProductService(IRepository repository) : IProductService
{

    public async Task<IEnumerable<ProductsDto>> GetAllProducts()
    {
        var productsDb = await repository.GetAllProducts().ConfigureAwait(false);
        var productDtos = productsDb.Select(b => b.ToProductsDto()).ToList();
        return productDtos;
    }

}
