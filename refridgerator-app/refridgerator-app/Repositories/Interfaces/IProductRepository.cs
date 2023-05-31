using refridgerator_app.Models;

namespace refridgerator_app.Repositories.Interfaces
{
    internal interface IProductRepository
    {
        List<Product> GetAll();
        Product GetById(int id);
        List<Product> GetByName(string productName);
        int Insert(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
