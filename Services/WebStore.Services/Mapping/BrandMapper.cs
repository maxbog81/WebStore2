using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandMapper
    {
        public static BrandDTO ToDTO(this Brand Brand) => Brand is null ? null : new BrandDTO
        {
            Id = Brand.Id,
            Name = Brand.Name,
            Order = Brand.Order,
            ProductsCount = Brand.Products?.Count ?? 0
        };

        public static Brand FromDTO(this BrandDTO Brand) => Brand is null ? null : new Brand
        {
            Id = Brand.Id,
            Name = Brand.Name,
            Order = Brand.Order,
        };

        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> Brands) => Brands?.Select(ToDTO);

        public static IQueryable<BrandDTO> ToDTO(this IQueryable<Brand> Brands) => Brands?.Select(Brand => new BrandDTO
        {
            Id = Brand.Id,
            Name = Brand.Name,
            Order = Brand.Order,
            ProductsCount = Brand.Products.Count(),
        });
    }
}