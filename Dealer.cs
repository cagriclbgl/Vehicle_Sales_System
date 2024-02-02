using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Spare_Parts_Sales
{
    public class Dealer : User
    {
        private DatabaseManager dbManager;

        public Dealer(string userName, string password, string email, string phoneNumber, DatabaseManager dbManager)
            : base(userName, password, email, phoneNumber, "Dealer")
        {
            this.dbManager = dbManager;
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

        public void AddCar()
        {
            Console.WriteLine("Enter car model:");
            string model = Console.ReadLine();

            Console.WriteLine("Enter stock quantity:");
            int stock = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter price:");
            decimal price = decimal.Parse(Console.ReadLine());

            dbManager.AddCar(model, stock, price);
            Console.WriteLine($"New car {model} added successfully.");
        }

        public void UpdateCarStock()
        {
            Console.Write("Enter the Car ID to update stock: ");
            if (int.TryParse(Console.ReadLine(), out int carId))
            {
                Console.Write("Enter the new stock quantity: ");
                if (int.TryParse(Console.ReadLine(), out int newStock))
                {
                    dbManager.UpdateStock(carId, newStock);
                    Console.WriteLine("Car stock updated successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid stock quantity. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Car ID. Please enter a valid number.");
            }
        }

        public void UpdateCarPrice()
        {
            Console.Write("Enter the Car ID to update price: ");
            if (int.TryParse(Console.ReadLine(), out int carId))
            {
                Console.Write("Enter the new price: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                {
                    dbManager.UpdatePrice(carId, newPrice);
                    Console.WriteLine("Car price updated successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid price. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Car ID. Please enter a valid number.");
            }
        }
    }
}
