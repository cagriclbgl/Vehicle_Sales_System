using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Vehicle_Spare_Parts_Sales
{
    internal class Admin : User
    {
        private DatabaseManager dbManager;

       
        public Admin(string userName, string password, string email, string phoneNumber, DatabaseManager dbManager)
            : base(userName, password, email, phoneNumber, "Admin")
        {
            this.dbManager = dbManager;
        }

        public void ListUsers()
        {
            dbManager.ListUsers(); 
        }

        public void DeleteUser(string userName)
        {
            dbManager.DeleteUser(userName);  
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
