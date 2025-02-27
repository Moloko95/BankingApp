using BankingApp.Models;
using BankingApp.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BankingApp.ViewModels
{
    public class ProductsViewModel : BindableObject
    {
        private readonly DatabaseService dbService;

        public ObservableCollection<Product> Products { get; set; }

        public ICommand AddToCartCommand { get; }

        public ProductsViewModel()
        {
            dbService = new DatabaseService();  
            Products = new ObservableCollection<Product>();  

            LoadProducts();  

            AddToCartCommand = new Command<Product>(AddToCart);
        }

        private void LoadProducts()
        {
            var db = dbService.GetConnection();
            var dbProducts = db.Table<Product>().ToList();

            if (dbProducts.Any())
            {
                Products = new ObservableCollection<Product>(dbProducts);
            }
            else
            {
                Products = new ObservableCollection<Product>
                {
                    new Product { ProductId = 1, ProductName = "Apple", Price = 15.00, ImageUrl = "apples.jpg" },
                    new Product { ProductId = 2, ProductName = "Banana", Price = 12.00, ImageUrl = "banana.png" },
                    new Product { ProductId = 3, ProductName = "Grapes", Price = 35.00, ImageUrl = "grapes.jpg" },
                    new Product { ProductId = 4, ProductName = "Orange", Price = 20.00, ImageUrl = "orange.jpg" },
                    new Product { ProductId = 5, ProductName = "Watermelon", Price = 18.00, ImageUrl = "watermellon.jpg" }
                };

                db.InsertAll(Products);
            }
        }

        private void AddToCart(Product product)
        {
            if (product == null) return;

            var db = dbService.GetConnection();
            var cartItem = db.Table<ShoppingCart>().FirstOrDefault(c => c.ProductId == product.ProductId);

            if (cartItem == null)
            {
                cartItem = new ShoppingCart
                {
                    ProfileId = 1,  
                    ProductId = product.ProductId,
                    Quantity = 1
                };
                db.Insert(cartItem);
            }
            else
            {
                cartItem.Quantity += 1;
                db.Update(cartItem);
            }

            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            mainPage?.DisplayAlert("Success", $"{product.ProductName} added to cart!", "OK");
        }
    }
}
