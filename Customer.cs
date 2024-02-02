using System;
using System.Collections.Generic;

namespace Vehicle_Spare_Parts_Sales
{
    internal class Customer : User
    {
        public Cart ShoppingCart { get; private set; } = new Cart();
        private DatabaseManager dbManager;

        public Customer(string userName, string password, string email, string phoneNumber, DatabaseManager dbManager)
            : base(userName, password, email, phoneNumber, "Customer")
        {
            this.dbManager = dbManager;
        }

        public void AddCarToCart(int carId, int quantity)
        {
            Car car = dbManager.ListCars().FirstOrDefault(c => c.CarId == carId);
            if (car != null && car.Stock >= quantity)
            {
                // Sepete ekleme işlemi
                ShoppingCart.AddToCart(car, quantity);
                Console.WriteLine($"{quantity} {car.Model} added to the cart.");
            }
            else
            {
                Console.WriteLine("Not enough stock or car not found.");
            }
        }

        public void Checkout()
        {
            foreach (var item in ShoppingCart.Items)
            {
                Car car = item.Key;
                int quantity = item.Value;

                // Stok kontrolü
                if (car.Stock >= quantity)
                {
                    // Stok güncelleme
                    dbManager.UpdateCarStock(car.CarId, car.Stock - quantity);
                    Console.WriteLine($"Purchased {quantity} of {car.Model}. Stock updated.");
                }
                else
                {
                    Console.WriteLine($"Not enough stock for {car.Model}.");
                }
            }

            // Sepeti temizle
            ShoppingCart.ClearCart();
        }
        public void ViewCart()
        {
            if (ShoppingCart.Items.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            Console.WriteLine("\nYour Cart:");
            decimal total = 0;
            foreach (var item in ShoppingCart.Items)
            {
                Car car = item.Key;
                int quantity = item.Value;
                decimal subtotal = car.Price * quantity;
                total += subtotal;

                Console.WriteLine($"{quantity} x {car.Model} (Price: {car.Price} each) - Subtotal: {subtotal}");
            }

            Console.WriteLine($"Total: {total}");
        }


        public void ViewAvailableCars()
        {
            List<Car> cars = dbManager.ListCars();
            Console.WriteLine("Available cars:");
            foreach (var car in cars)
            {
                Console.WriteLine($"{car.Model} - Stock: {car.Stock} - Price: {car.Price}");
            }
        }



    }
}
