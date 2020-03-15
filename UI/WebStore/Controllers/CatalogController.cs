using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private const string __PageSize = "PageSize";

        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, IConfiguration Configuration)
        {
            _ProductData = ProductData;
            _Configuration = Configuration;
        }

        public IActionResult Shop(int? SectionId, int? BrandId, [FromServices] IMapper Mapper, int Page = 1)
        {
            var page_size = int.TryParse(_Configuration[__PageSize], out var size) ? size : (int?)null;

            var products = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Page = Page,
                PageSize = page_size
            });

            return View(new CatalogViewModel
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Products = products.Products.Select(Mapper.Map<ProductViewModel>).OrderBy(p => p.Order),
                PageViewModel = new PageViewModel
                {
                    PageSize = page_size ?? 0,
                    PageNumber = Page,
                    TotalItems = products.TotalCount
                }
            });
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProductById(id);

            if (product is null)
                return NotFound();

            return View(new ProductViewModel
            {
                 Id = product.Id,
                 Name = product.Name,
                 Price = product.Price,
                 ImageUrl = product.ImageUrl,
                 Order = product.Order,
                 Brand = product.Brand?.Name
            });
        }

        #region API

        public IActionResult GetFiltratedItems(int? SectionId, int? BrandId, [FromServices] IMapper Mapper, int Page)
        {
            var products = GetProducts(SectionId, BrandId, Page);
            return PartialView("Partial/_FeaturesItem", products.Select(Mapper.Map<ProductViewModel>));
        }

        private IEnumerable<ProductDTO> GetProducts(int? SectionId, int? BrandId, int Page) =>
            _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Page = Page,
                PageSize = int.Parse(_Configuration[__PageSize])
            }).Products;

        #endregion
    }
}