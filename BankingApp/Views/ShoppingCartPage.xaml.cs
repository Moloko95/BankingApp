using BankingApp.Models;
using BankingApp.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BankingApp.Views
{
    public partial class ShoppingCartPage : ContentPage
    {
        private DatabaseService dbService;
        public ObservableCollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCartPage()
        {
            InitializeComponent();
            dbService = new DatabaseService();
            LoadShoppingCart();
        }

        private void LoadShoppingCart()
        {
            var db = dbService.GetConnection();
            var cartItems = from cart in db.Table<ShoppingCart>()
                            join product in db.Table<Product>() on cart.ProductId equals product.ProductId
                            select new ShoppingCartItem
                            {
                                ProductName = product.ProductName,
                                Quantity = cart.Quantity,
                                Price = product.Price
                            };

            ShoppingCartItems = new ObservableCollection<ShoppingCartItem>(cartItems.ToList());
            ShoppingCartListView.ItemsSource = ShoppingCartItems;
        }

        
    }
}
