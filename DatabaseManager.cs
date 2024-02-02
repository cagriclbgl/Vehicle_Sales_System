using System.Data.SqlClient;
using Vehicle_Spare_Parts_Sales;

public class DatabaseManager
{
    private string connectionString = "Data Source=CGR;Initial Catalog=salesSystem;Integrated Security=True"
;

    public DatabaseManager(string connectionString)
    {
        
    }

    public User GetUser(string userName, string password, DatabaseManager dbManager)
    {
        User user = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT UserName, Password, Email, PhoneNumber, UserType FROM Users WHERE UserName = @UserName AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string userType = reader["UserType"].ToString();
                            switch (userType)
                            {
                                case "Admin":
                                    user = new Admin(
                                        userName: reader["UserName"].ToString(),
                                        password: reader["Password"].ToString(),
                                        email: reader["Email"].ToString(),
                                        phoneNumber: reader["PhoneNumber"].ToString(),
                                        dbManager: dbManager
                                    );
                                    break;
                                case "Customer":
                                    user = new Customer(
                                        userName: reader["UserName"].ToString(),
                                        password: reader["Password"].ToString(),
                                        email: reader["Email"].ToString(),
                                        phoneNumber: reader["PhoneNumber"].ToString(),
                                        dbManager: dbManager
                                    );
                                    break;
                                case "Dealer":
                                    user = new Dealer(
                                        userName: reader["UserName"].ToString(),
                                        password: reader["Password"].ToString(),
                                        email: reader["Email"].ToString(),
                                        phoneNumber: reader["PhoneNumber"].ToString(),
                                        dbManager: dbManager
                                    );
                                    break;
                                default:
                                    user = null; 
                                    break;
                            }
                        }
                    }
                }
            }




        }
        catch (Exception ex)
        {
            Console.WriteLine("Bir hata oluştu: " + ex.Message);
            
        }
        return user;
    }


    public void AddCar(string model, int stock, decimal price)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Cars (Model, Stock, Price) VALUES (@Model, @Stock, @Price)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Model", model);
                    command.Parameters.AddWithValue("@Stock", stock);
                    command.Parameters.AddWithValue("@Price", price);

                    // Komutu çalıştır
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                // SQL ile ilgili hatalar için
                Console.WriteLine("SQL Error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // Diğer tüm hatalar için
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }


    public bool IsUserNameTaken(string userName)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", userName);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }

    
    public void AddUser(User user)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "INSERT INTO Users (UserName, Password, Email, PhoneNumber, UserType) VALUES (@UserName, @Password, @Email, @PhoneNumber, @UserType)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Password", user.Password); 
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@UserType", user.UserType); 

                command.ExecuteNonQuery();
            }
        }
        Console.WriteLine("New user added to database");
    }


   
    public void Delete(string tableName, string condition)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            
            string query = $"DELETE FROM {tableName} WHERE {condition}";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        Console.WriteLine($"{tableName} tablosundan koşula uygun veri(ler) silindi.");
    }

    public void Add(string tableName, Dictionary<string, string> data)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            
            string columns = string.Join(", ", data.Keys);
            string values = string.Join(", ", data.Values);

            string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        Console.WriteLine($"{tableName} tablosuna yeni veri eklendi.");
    }


    public void UpdateCarStock(int carId, int newStock)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "UPDATE Cars SET Stock = @NewStock WHERE CarId = @CarId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NewStock", newStock);
                command.Parameters.AddWithValue("@CarId", carId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateStock(int carId, int newStock)
    {
        string query = "UPDATE Cars SET Stock = @Stock WHERE CarId = @CarId";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Stock", newStock);
            command.Parameters.AddWithValue("@CarId", carId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }


    public void CompleteOrder(string username, Cart cart)
    {
        
        foreach (var item in cart.Items)
        {
            UpdateStock(item.Key.CarId, item.Key.Stock - item.Value);
            
            AddOrder(username, item.Key.CarId, item.Value, item.Key.Price * item.Value);
        }
    }


    public void AddOrder(string username, int carId, int quantity, decimal totalPrice)
    {
        string query = "INSERT INTO Orders (CustomerId, CarId, Quantity, TotalPrice) VALUES (@CustomerId, @CarId, @Quantity, @TotalPrice)";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerId", username);
            command.Parameters.AddWithValue("@CarId", carId);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@TotalPrice", totalPrice);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public Car GetCarById(int carId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM Cars WHERE CarId = @CarId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CarId", carId);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Car((int)reader["CarId"], reader["Model"].ToString(), (int)reader["Stock"], (decimal)reader["Price"]);
                }
                else
                {
                    return null; 
                }
            }
        }
    }

    public List<Car> ListCars()
    {
        List<Car> cars = new List<Car>();
        string query = "SELECT * FROM Cars";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                cars.Add(new Car(
                    (int)reader["CarId"],
                    reader["Model"].ToString(),
                    (int)reader["Stock"],
                    (decimal)reader["Price"]
                ));
            }
        }
        return cars;
    }


    public void AddCar(Car car)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Cars (Model, Stock, Price) VALUES (@Model, @Stock, @Price)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
              
                command.Parameters.AddWithValue("@Model", car.Model);
                command.Parameters.AddWithValue("@Stock", car.Stock);
                command.Parameters.AddWithValue("@Price", car.Price);

                
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine($"{car.Model} modeli veritabanına eklendi.");
            }
        }
    }


    public void UpdatePrice(int carId, decimal newPrice)
    {
        string query = "UPDATE Cars SET Price = @Price WHERE CarId = @CarId";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Price", newPrice);
            command.Parameters.AddWithValue("@CarId", carId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void ListUsers()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT UserName, Email, UserType FROM Users";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Username: {reader["UserName"]}, Email: {reader["Email"]}, UserType: {reader["UserType"]}");
                    }
                }
            }
        }
    }

    public void DeleteUser(string userName)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Users WHERE UserName = @UserName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", userName);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"User {userName} has been successfully deleted.");
                }
                else
                {
                    Console.WriteLine($"User {userName} not found.");
                }
            }
        }
    }





}



