using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using test_api.Interfaces.Repositories;
using test_api.Model;
using test_api.Repositories;
using test_api.Services;

namespace test_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        public ProductController(IMemoryCache memoryCache, IProductRepository productRepository)
        {
            this._memoryCache = memoryCache;
            this._productRepository = productRepository;
        }

        [HttpGet]
        // [Authorize]
        [Route("ProductsList")]
        public async Task<IEnumerable<Product>> Get()
        {
            IEnumerable<Product> products;
            bool isExist = _memoryCache.TryGetValue("Products", out products);
            if (!isExist)
            {
                products = await this._productRepository.GetProducts();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20));
                _memoryCache.Set("Products", products, cacheEntryOptions);
            }
            return products;
        }
        [HttpGet]
        [Route("ProductDetail")]
        public async Task<Product> Get(int id)
        {
            Product product;
            product = await this._productRepository.GetProductById(id);
            return product;
        }
        //[HttpPost]
        //[Route("CreateProduct")]
        //public async Task<ActionResult<Product>> POST(Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();
        //    _cacheService.RemoveData("Product");
        //    return CreatedAtAction(nameof(Get), new
        //    {
        //        id = product.ProductId
        //    }, product);
        //}
        [HttpPost]
        [Route("DeleteProduct")]
        public async Task Delete(int id)
        {
            await this._productRepository.DeleteProduct(id);
        }
        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<Int32> Update(int id, Product product)
        {
            return await this._productRepository.UpdateProduct(id, product);
        }
    }
}
