using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Spare_Parts_Sales
{

    public class Car
    {
        public int CarId { get; set; }
        public string Model { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }

        public Car(int carId, string model, int stock, decimal price)
        {
            CarId = carId;
            Model = model;
            Stock = stock;
            Price = price;
        }
    }





}
