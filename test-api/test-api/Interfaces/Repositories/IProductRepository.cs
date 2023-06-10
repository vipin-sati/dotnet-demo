using test_api.Model;

namespace test_api.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProductById(int productId);
        int AddProduct(Product product);
        Task<Int32> UpdateProduct(int productId, Product product);
        Task DeleteProduct(int productId);
    }
}
