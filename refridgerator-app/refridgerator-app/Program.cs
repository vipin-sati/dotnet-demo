// See https://aka.ms/new-console-template for more information
using refridgerator_app;
using refridgerator_app.Repositories;
using refridgerator_app.Repositories.Interfaces;
using refridgerator_app.Models;

IProductRepository productRepository = new ProductRepository();
var app = new Refrigerator(productRepository);

// Stage 1: Inserting and consuming products
app.InsertProduct("Sugar", 20, DateTime.Now.AddDays(15));
app.InsertProduct("Milk", 6, DateTime.Now.AddDays(20));
app.InsertProduct("Tea", 10, DateTime.Now.AddDays(120));
app.InsertProduct("Bread", 1, DateTime.Now.AddDays(5));
app.InsertProduct("Ginger", 1, DateTime.Now.AddDays(5));
app.ConsumeProduct("Tea", 1);
app.ConsumeProduct("Sugar", 3);
app.ShowCurrentStatus();

// Stage 2: Checking expiry
app.InsertProduct("Yogurt", 4, DateTime.Now.AddDays(2));

// Subscribe to the Product Expiring event
app.ProductExpiring += App_ProductExpiring;

app.CheckExpiry();
app.ShowCurrentStatus();

// Stage 3: Creating a shopping list
app.CreateShoppingList();


void App_ProductExpiring(object? sender, ProductEventArgs e)
{
    Product expiringProduct = e.ExpiringProduct;
    Console.WriteLine($"Alert: Item {expiringProduct.Name} is about to expire!");
}
