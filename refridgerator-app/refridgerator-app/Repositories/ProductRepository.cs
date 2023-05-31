using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using refridgerator_app.Models;
using refridgerator_app.Repositories.Interfaces;

namespace refridgerator_app.Repositories
{
    internal class ProductRepository : IProductRepository
    {

        private List<Product> products;
        private int identity;

        public ProductRepository()
        {
            products = new List<Product>();
            identity = 1;
        }

        public List<Product> GetAll()
        {
            return products;
        }

        public Product GetById(int id)
        {
            return products.Find(p => p.Id == id);
        }

        public List<Product> GetByName(string productName)
        {
            return products.Where(p => p.Name == productName && p.ExpirationDate >= DateTime.Now).OrderBy(p => p.ExpirationDate).ToList<Product>();
        }

        public int Insert(Product product)
        {
            product.Id = identity++;
            products.Add(product);
            return product.Id;
        }

        public void Update(Product product)
        {
            var existingProduct = products.Find(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Quantity = product.Quantity;
                existingProduct.ExpirationDate = product.ExpirationDate;
            }
        }

        public void Delete(int id)
        {
            var product = products.Find(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
            }
        }
    }
}
