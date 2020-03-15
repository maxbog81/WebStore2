using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration config) : base(config, WebAPI.Products) { }

        public IEnumerable<SectionDTO> GetSections() => Get<List<SectionDTO>>($"{_ServiceAddress}/sections");

        public SectionDTO GetSectionById(int id) => Get<SectionDTO>($"{_ServiceAddress}/sections/{id}");

        public IEnumerable<BrandDTO> GetBrands() => Get<List<BrandDTO>>($"{_ServiceAddress}/brands");

        public BrandDTO GetBrandById(int id) => Get<BrandDTO>($"{_ServiceAddress}/brands/{id}");

        public PagedProductsDTO GetProducts(ProductFilter Filter = null) => 
            Post(_ServiceAddress, Filter)
               .Content
               .ReadAsAsync<PagedProductsDTO>()
               .Result;

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_ServiceAddress}/{id}");
    }
}
