using System;
using System.Collections.Generic;
using System.Linq;

namespace Vehicle_Spare_Parts_Sales
{
    public class Cart
    {
        
        public Dictionary<Car, int> Items { get; private set; } = new Dictionary<Car, int>();

       
        public void AddToCart(Car car, int quantity)
        {
            if (car == null)
            {
                Console.WriteLine("Invalid car.");
                return;
            }

            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be a positive number.");
                return;
            }

            if (car.Stock >= quantity)
            {
                if (Items.ContainsKey(car))
                {
                    Items[car] += quantity; 
                }
                else
                {
                    Items.Add(car, quantity); 
                }
                Console.WriteLine($"{quantity} units of {car.Model} have been added to the cart.");
            }
            else
            {
                Console.WriteLine("Insufficient stock available.");
            }
        }

        public void ViewCart()
        {
            if (Items.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            Console.WriteLine("Items in your cart:");
            decimal total = 0;
            foreach (var item in Items)
            {
                decimal itemTotal = item.Key.Price * item.Value;
                total += itemTotal;
                Console.WriteLine($"{item.Key.Model}: {item.Value} units - Total: {itemTotal:C}");
            }
            Console.WriteLine($"Total Cart Amount: {total:C}");
        }

        public void ClearCart()
        {
            Items.Clear();
            Console.WriteLine("Cart has been cleared.");
        }

        public decimal CalculateTotal()
        {
            return Items.Sum(item => item.Key.Price * item.Value);
        }
    }
}
