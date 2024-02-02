using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Spare_Parts_Sales
{
    public class User
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string UserType { get; protected set; } 

        public User(string userName, string password, string email, string phoneNumber, string userType)
        {
            UserName = userName;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
            UserType = userType; 
        }

    }
}
