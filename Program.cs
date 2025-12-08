using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CarShowroomCSV
{
    internal class Program
    {
        struct Car
        {
            public int Id;
            public string Name;
            public double Price;
            public int Year;
            public string Type;

            public Car(int id, string name, double price, int year, string type)
            {
                Id = id;
                Name = name;
                Price = price;
                Year = year;
                Type = type;
            }

            public override string ToString() => $"{Id},{Name},{Price},{Year},{Type}";

            public static Car? FromCsv(string line)
            {
                try
                {
                    var parts = line.Split(',');
                    if (parts.Length != 5) return null;
                    return new Car(
                        int.Parse(parts[0]),
                        parts[1],
                        double.Parse(parts[2]),
                        int.Parse(parts[3]),
                        parts[4]
                    );
                }
                catch
                {
                    return null;
                }
            }
        }

        struct User
        {
            public int Id;
            public string Email;
            public string PasswordHash;

            public User(int id, string email, string passwordHash)
            {
                Id = id;
                Email = email;
                PasswordHash = passwordHash;
            }

            public override string ToString() => $"{Id},{Email},{PasswordHash}";

            public static User? FromCsv(string line)
            {
                try
                {
                    var parts = line.Split(',');
                    if (parts.Length != 3) return null;
                    return new User(
                        int.Parse(parts[0]),
                        parts[1],
                        parts[2]
                    );
                }
                catch
                {
                    return null;
                }
            }
        }

        static List<Car> cars = new List<Car>();
        static List<User> users = new List<User>();
        static bool isManager = false;
        const string managerPassword = "123";
        const string carsFile = "cars.csv";
        const string usersFile = "users.csv";

        static void Main()
        {
            LoadCars();
            LoadUsers();
            ChooseRole();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("WELCOME TO CARSHOWROOM CSV");
                Console.WriteLine(isManager ? "Manager menu:" : "Client menu:");
                if (isManager)
                {
                    Console.WriteLine("1. Show Cars 2. Buy Car 3. Add Car 4. Remove Car 5. Stats 6. Search 7. Sort 8. Exit");
                }
                else
                {
                    Console.WriteLine("1. Show Cars 2. Buy Car 3. Stats 4. Search 5. Exit");
                }

                Console.Write("Select option: ");
                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

                if (isManager)
                {
                    switch (choice)
                    {
                        case 1: ShowCars(); break;
                        case 2: BuyCar(); break;
                        case 3: AddCar(); break;
                        case 4: RemoveCar(); break;
                        case 5: ShowStats(); break;
                        case 6: SearchCar(); break;
                        case 7: SortCars(); break;
                        case 8: exit = true; break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case 1: ShowCars(); break;
                        case 2: BuyCar(); break;
                        case 3: ShowStats(); break;
                        case 4: SearchCar(); break;
                        case 5: exit = true; break;
                    }
                }
            }
        }

        static void LoadCars()
        {
            if (!File.Exists(carsFile))
            {
                File.WriteAllText(carsFile, "Id,Name,Price,Year,Type\n");
                return;
            }
            cars.Clear();
            var lines = File.ReadAllLines(carsFile).Skip(1);
            foreach (var line in lines)
            {
                var car = Car.FromCsv(line);
                if (car.HasValue) cars.Add(car.Value);
            }
        }

        static void SaveCars()
        {
            var lines = new List<string> { "Id,Name,Price,Year,Type" };
            lines.AddRange(cars.Select(c => c.ToString()));
            File.WriteAllLines(carsFile, lines);
        }

        static void LoadUsers()
        {
            if (!File.Exists(usersFile))
            {
                File.WriteAllText(usersFile, "Id,Email,PasswordHash\n");
                return;
            }
            users.Clear();
            var lines = File.ReadAllLines(usersFile).Skip(1);
            foreach (var line in lines)
            {
                var user = User.FromCsv(line);
                if (user.HasValue) users.Add(user.Value);
            }
        }

        static void SaveUsers()
        {
            var lines = new List<string> { "Id,Email,PasswordHash" };
            lines.AddRange(users.Select(u => u.ToString()));
            File.WriteAllLines(usersFile, lines);
        }

        static int NextCarId() => cars.Count == 0 ? 1 : cars.Max(c => c.Id) + 1;
        static int NextUserId() => users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;

        static void ChooseRole()
        {
            while (true)
            {
                Console.WriteLine("1 - Client 2 - Manager");
                string choice = Console.ReadLine();
                if (choice == "1") { isManager = false; AuthUser(); return; }
                if (choice == "2")
                {
                    isManager = true;
                    Console.Write("Enter manager password: ");
                    if (ReadPassword() == managerPassword) return;
                    Console.WriteLine("Wrong password.");
                }
            }
        }

        static void AuthUser()
        {
            while (true)
            {
                Console.WriteLine("1-Login 2-Register");
                string cmd = Console.ReadLine();
                if (cmd == "1") { if (Login()) return; }
                if (cmd == "2") Register();
            }
        }

        static bool Login()
        {
            Console.Write("Email: "); string email = Console.ReadLine();
            Console.Write("Password: "); string password = ReadPassword();
            string hash = ComputeHash(password);
            var user = users.FirstOrDefault(u => u.Email == email && u.PasswordHash == hash);
            if (user.Email != null) return true;
            Console.WriteLine("Login failed."); return false;
        }

        static void Register()
        {
            Console.Write("Email: "); string email = Console.ReadLine();
            if (users.Any(u => u.Email == email)) { Console.WriteLine("User exists."); return; }
            Console.Write("Password: "); string password = ReadPassword();
            users.Add(new User(NextUserId(), email, ComputeHash(password)));
            SaveUsers();
            Console.WriteLine("Registered successfully.");
        }

        static string ReadPassword()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && sb.Length > 0) { sb.Length--; Console.Write("\b \b"); }
                else { sb.Append(key.KeyChar); Console.Write("*"); }
            }
            Console.WriteLine();
            return sb.ToString();
        }

        static string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        static void ShowCars()
        {
            Console.WriteLine("Cars:");
            Console.WriteLine("ID | Name | Price | Year | Type");
            foreach (var c in cars)
            {
                Console.WriteLine($"{c.Id} | {c.Name} | {c.Price} | {c.Year} | {c.Type}");
            }
            Console.WriteLine("Press any key..."); Console.ReadKey();
        }

        static void AddCar()
        {
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("Price: "); double price = double.Parse(Console.ReadLine());
            Console.Write("Year: "); int year = int.Parse(Console.ReadLine());
            Console.Write("Type: "); string type = Console.ReadLine();
            cars.Add(new Car(NextCarId(), name, price, year, type));
            SaveCars();
        }

        static void RemoveCar()
        {
            Console.Write("Enter Car ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;
            cars.RemoveAll(c => c.Id == id);
            SaveCars();
        }

        static void BuyCar()
        {
            Console.Write("Enter Car ID to buy: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;
            var car = cars.FirstOrDefault(c => c.Id == id);
            if (car.Name == null) { Console.WriteLine("Car not found."); return; }
            Console.WriteLine($"Purchased {car.Name} for {car.Price}$");
        }

        static void ShowStats()
        {
            if (cars.Count == 0) { Console.WriteLine("No cars."); Console.ReadKey(); return; }
            Console.WriteLine($"Total cars: {cars.Count}");
            Console.WriteLine($"Min price: {cars.Min(c => c.Price)}$");
            Console.WriteLine($"Max price: {cars.Max(c => c.Price)}$");
            Console.WriteLine($"Sum price: {cars.Sum(c => c.Price)}$");
            Console.WriteLine($"Average price: {cars.Average(c => c.Price):F2}$");
            Console.WriteLine("Press any key..."); Console.ReadKey();
        }

        static void SearchCar()
        {
            Console.Write("Search term: ");
            string q = Console.ReadLine().ToLower();
            var result = cars.Where(c => c.Name.ToLower().Contains(q));
            foreach (var c in result)
                Console.WriteLine($"{c.Id} | {c.Name} | {c.Price} | {c.Year} | {c.Type}");
            Console.WriteLine("Press any key..."); Console.ReadKey();
        }

        static void SortCars()
        {
            Console.WriteLine("1-By Price 2-By Year");
            string choice = Console.ReadLine();
            if (choice == "1") cars = cars.OrderBy(c => c.Price).ToList();
            else if (choice == "2") cars = cars.OrderBy(c => c.Year).ToList();
            SaveCars();
        }
    }
}
