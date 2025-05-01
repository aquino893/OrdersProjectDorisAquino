using OrdersProjectDorisAquino.Application.DTOs;
using OrdersProjectDorisAquino.Domain.Entities;
using System;

namespace OrdersProjectDorisAquino.Application.Mapping;

public static class ProductMapping
{
    public static ProductsDto ToProductsDto(this Product product)
    {
        var productDto = new ProductsDto
        {
            ProductId = product.ProductID,
            ProductName = product.ProductName,
            UnitPrice = decimal.Round(product.UnitPrice, 2, MidpointRounding.AwayFromZero)
        };
        return productDto;
    }
}
