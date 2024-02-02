using System.Text.RegularExpressions;
using Vehicle_Spare_Parts_Sales;
//210229030_Cagri_Celebioglu
public class Program
{
    private static DatabaseManager dbManager = new DatabaseManager("Data Source=CGR;Initial Catalog=salesSystem;Integrated Security=True");

    public static void Main()
    {

      

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Giriş Menüsü ---");
            Console.WriteLine("1: Giriş Yap");
            Console.WriteLine("2: Kayıt Ol");
            Console.WriteLine("3: Çıkış");
            Console.Write("Seçiminiz: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    running = false;
                    Console.WriteLine("Programdan çıkılıyor...");
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim, tekrar deneyin.");
                    break;
            }
        }
    }

    private static void Login()
    {
        Console.Write("Kullanıcı adı : ");
        string userName = Console.ReadLine();

        Console.Write("Şifre : ");
        string password = Console.ReadLine();

      
        string hashedPassword = HashPassword(password);

       
        var user = dbManager.GetUser(userName, hashedPassword,dbManager);
        if (user != null)
        {
            Console.WriteLine("Başarıyla giriş yapıldı !");

            
            switch (user.UserType)
            {
                case "Admin":
                    Admin admin = user as Admin;
                    if (admin != null)
                    {
                        bool exitMenu = false;
                        while (!exitMenu)
                        {
                            Console.WriteLine("\n--- Admin Menu ---");
                            Console.WriteLine("1: List Users");
                            Console.WriteLine("2: Delete Users");
                            Console.WriteLine("3: List Products");
                            Console.WriteLine("4: Exit");

                            Console.Write("Select an option: ");
                            string choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    admin.ListUsers();
                                    break;
                                case "2":
                                    Console.Write("Enter the username of the user to delete: ");
                                    string usernameToDelete = Console.ReadLine();
                                    admin.DeleteUser(usernameToDelete);

                                    break;
                                case "3":
                                    admin.ViewAvailableCars();
                                    break;
                                case "4":
                                    exitMenu = true;
                                    Console.WriteLine("Exiting admin menu...");
                                    break;
                                default:
                                    Console.WriteLine("Invalid option, please try again.");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: The logged-in user is not an Admin!!");
                    }
                    break;


                case "Customer":
                    Customer customer = user as Customer;
                    if (customer != null)
                    {
                        bool exitMenu = false;
                        while (!exitMenu)
                        {
                            Console.WriteLine("\n--- Customer Menu ---");
                            Console.WriteLine("1. View Available Cars");
                            Console.WriteLine("2. Add Car to Cart");
                            Console.WriteLine("3. View Cart");
                            Console.WriteLine("4. Checkout");
                            Console.WriteLine("5. Exit");
                            Console.Write("Select an option: ");
                            string option = Console.ReadLine();

                            switch (option)
                            {
                                case "1":
                                    customer.ViewAvailableCars();
                                    break;
                                case "2":
                                    Console.Write("Enter Car ID: ");
                                    int carId = int.Parse(Console.ReadLine());
                                    Console.Write("Enter Quantity: ");
                                    int quantity = int.Parse(Console.ReadLine());
                                    customer.AddCarToCart(carId, quantity);  
                                    break;
                                case "3":
                                    customer.ViewCart(); 
                                    break;
                                case "4":
                                    customer.Checkout(); 
                                    break;
                                case "5":
                                    exitMenu = true;
                                    Console.WriteLine("Exiting customer menu...");
                                    break;
                                default:
                                    Console.WriteLine("Invalid option, please try again.");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Logged in user is not a Customer.");
                    }
                    break;

                case "Dealer":
                    Dealer dealer = user as Dealer;
                    if (dealer != null)
                    {
                        bool exitMenu = false;
                        while (!exitMenu)
                        {
                            Console.WriteLine("\nDealer Menu:");
                            Console.WriteLine("1. View Available Cars");
                            Console.WriteLine("2. Update Car Stock");
                            Console.WriteLine("3. Update Car Price");
                            Console.WriteLine("4. Exit");
                            Console.Write("Select an option: ");
                            string option = Console.ReadLine();

                            switch (option)
                            {
                                case "1":
                                    dealer.ViewAvailableCars();
                                    break;
                                case "2":
                                    dealer.AddCar();
                                    break;
                                case "3":
                                    dealer.UpdateCarPrice();
                                    break;
                                case "4":
                                    return;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Logged in user is not a Customer.");
                    }
                    break;
                default:
                    Console.WriteLine("Undefined user type.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid username or password!");
        }
    }

    private static string HashPassword(string password)
    {
        // NOTE: This is a simple example. In a real application, a secure hashing algorithm should be used.
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }



    private static void Register()
    {
        Console.Write("Kullanıcı Adı (5-20 karakter, ilk harf alfabetik): ");
        string userName = Console.ReadLine();

        Console.Write("Şifre (8-20 karakter, en az bir rakam, bir büyük harf, bir küçük harf ve bir özel karakter): ");
        string password = Console.ReadLine();

        Console.Write("E-posta (Geçerli bir e-posta formatında): ");
        string email = Console.ReadLine();

        Console.Write("Telefon Numarası (xxxx-xxx-xxxx formatında): ");
        string phoneNumber = Console.ReadLine();

        Console.WriteLine("Kayıt Olmak İstediğiniz Kullanıcı Türünü Seçin: ");
        Console.WriteLine("1: Müşteri (Customer)");
        Console.WriteLine("2: Satıcı (Dealer)");
        string userTypeChoice = Console.ReadLine();
        string userType = "";

        if (userTypeChoice == "1")
        {
            userType = "Customer";
        }
        else if (userTypeChoice == "2")
        {
            userType = "Dealer";
        }
        else
        {
            Console.WriteLine("Geçersiz seçim.");
            return;
        }

        
        if (!ValidateUserName(userName) || !ValidatePassword(password) ||
            !ValidateEmail(email) || !ValidatePhoneNumber(phoneNumber))
        {
            Console.WriteLine("Kayıt bilgileri geçersiz!");
            return;
        }

        if (dbManager.IsUserNameTaken(userName))
        {
            Console.WriteLine("Bu kullanıcı adı zaten alınmış!");
            return;
        }

        
        string hashedPassword = HashPassword(password);

        User newUser = new User(userName, hashedPassword, email, phoneNumber, userType);
        dbManager.AddUser(newUser);
        Console.WriteLine($"{userType} olarak kullanıcı başarıyla kaydedildi!");
    }


    
    private static bool ValidateUserName(string userName)
    {
        return Regex.IsMatch(userName, "^[a-zA-Z][a-zA-Z0-9]{4,19}$");
    }

  
    private static bool ValidatePassword(string password)
    {
        return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%&*\-+])[A-Za-z\d!@#$%&*\-+]{8,20}$");
    }


    private static bool ValidateEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }


    private static bool ValidatePhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^\d{4}-\d{3}-\d{4}$");
    }

}
