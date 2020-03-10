using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Product
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _db;

        public SqlProductData(WebStoreContext db) => _db = db;

        public IEnumerable<Section> GetSections() => _db.Sections
           //.Include(section => section.Products)
           .AsEnumerable();

        public SectionDTO GetSectionById(int id) => _db.Sections.Find(id).ToDTO();


        public IEnumerable<Brand> GetBrands() => _db.Brands
           //.Include(brand => brand.Products)
           .AsEnumerable();

        public BrandDTO GetBrandById(int id) => _db.Brands.Find(id).ToDTO();


        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Domain.Entities.Product> query = _db.Products;

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            return query
                .AsEnumerable()
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Order = p.Order,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Brand = p.Brand is null ? null : new BrandDTO
                    {
                        Id = p.Brand.Id,
                        Name = p.Brand.Name
                    },
                    Section = p.Section is null ? null : new SectionDTO
                    {
                        Id = p.Section.Id,
                        Name = p.Section.Name
                    }
                });
        }

        public ProductDTO GetProductById(int id)
        {
            var product = _db.Products
               .Include(p => p.Brand)
               .Include(p => p.Section)
               .FirstOrDefault(p => p.Id == id);
            return product is null ? null : new ProductDTO
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
        }
    }
}
