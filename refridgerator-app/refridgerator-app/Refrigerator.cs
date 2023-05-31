using refridgerator_app.Models;
using refridgerator_app.Repositories.Interfaces;

namespace refridgerator_app
{
    internal class Refrigerator
    {
        private readonly IProductRepository _productRepository;

        public event EventHandler<ProductEventArgs> ProductExpiring;

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
            var products = _productRepository.GetByName(productName);
            if (products != null)
            {
                if(quantity <= products.Sum(p => p.Quantity))
                {
                    foreach (var product in products)
                    {
                        if (product.Quantity >= quantity)
                        {
                            product.Quantity -= quantity;
                        }
                        else
                        {
                            product.Quantity = 0;
                            quantity -= product.Quantity;
                        }

                        _productRepository.Update(product);
                    }

                    Console.WriteLine($"Consumed {quantity} of {productName}.");
                }
                else
                {
                    Console.WriteLine($"Insufficient quantity of {productName} in the refrigerator.");
                }
            }
            else
            {
                Console.WriteLine($"Product {productName} not found in the refrigerator.");
            }
        }

        public void ShowCurrentStatus()
        {
            var products = _productRepository.GetAll().Where(p => p.ExpirationDate >= DateTime.Now);
            Console.WriteLine("Current Status:");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Name}: {product.Quantity} ({product.ExpirationDate.ToShortDateString()})");
            }
        }

        public void CheckExpiry()
        {
            var products = _productRepository.GetAll();
            TimeSpan remainingTime;
            foreach (var product in products)
            {
                remainingTime = product.ExpirationDate.Date - DateTime.Today;

                if (product.ExpirationDate.Date < DateTime.Today)
                {
                    Console.WriteLine($"Remove {product.Name} from the refrigerator. It has expired.");
                    _productRepository.Delete(product.Id);
                }
                else if (remainingTime.TotalDays <= 3) // Change the threshold as per your requirement
                {
                    OnProductExpiring(new ProductEventArgs(product));
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
        protected virtual void OnProductExpiring(ProductEventArgs e)
        {
            ProductExpiring?.Invoke(this, e);
        }
    }
}
