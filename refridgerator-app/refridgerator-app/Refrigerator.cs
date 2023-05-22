using refridgerator_app.Models;
using refridgerator_app.Repositories.Interfaces;

namespace refridgerator_app
{
    internal class Refrigerator
    {
        private readonly IProductRepository _productRepository;

        public Refrigerator(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public void InsertProduct(string name, decimal quantity, DateTime expirationDate)
        {
            var product = new Product
            {
                Name = name,
                Quantity = quantity,
                ExpirationDate = expirationDate
            };

            _productRepository.Insert(product);
        }

        public void ConsumeProduct(string productName, decimal quantity)
        {
            var product = _productRepository.GetByName(productName);
            if (product != null)
            {
                if (quantity <= product.Quantity)
                {
                    product.Quantity -= quantity;
                    Console.WriteLine($"Consumed {quantity} of {product.Name}.");
                    _productRepository.Update(product);
                }
                else
                {
                    Console.WriteLine($"Insufficient quantity of {product.Name} in the refrigerator.");
                }
            }
            else
            {
                Console.WriteLine($"Product {productName} not found in the refrigerator.");
            }
        }

        public void ShowCurrentStatus()
        {
            var products = _productRepository.GetAll();
            Console.WriteLine("Current Status:");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Name}: {product.Quantity} ({product.ExpirationDate.ToShortDateString()})");
            }
        }

        public void CheckExpiry()
        {
            var products = _productRepository.GetAll();
            foreach (var product in products)
            {
                if (product.ExpirationDate.Date < DateTime.Today)
                {
                    Console.WriteLine($"Remove {product.Name} from the refrigerator. It has expired.");
                    _productRepository.Delete(product.Id);
                }
            }
        }

        public void CreateShoppingList()
        {
            var products = _productRepository.GetAll();
            var shoppingList = new List<Product>();

            foreach (var product in products)
            {
                if (product.Quantity <= 1)
                {
                    shoppingList.Add(product);
                }
            }

            Console.WriteLine("Shopping List:");
            foreach (var product in shoppingList)
            {
                Console.WriteLine(product.Name);
            }
        }
    }
}
