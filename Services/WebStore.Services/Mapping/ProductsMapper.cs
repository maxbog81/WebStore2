using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.DTO.Products;

namespace WebStore.Services.Mapping
{
    public static class ProductMapper
    {
        public static ProductDTO ToDTO(this Domain.Entities.Product product) => product is null ? null : new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand is null ? null : new BrandDTO
            {
                Id = product.Brand.Id,
                Name = product.Brand.Name
            },
            Section = product.Section is null ? null : new SectionDTO
            {
                Id = product.Section.Id,
                Name = product.Section.Name
            }
        };

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Domain.Entities.Product> Products) =>
            Products?.Select(ToDTO);
    }
}
