using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CarShowroom
{
    internal class Program
    {
        static List<Car> cars = new List<Car>();
        static List<User> users = new List<User>();
        static List<Order> orders = new List<Order>();

        static string currentUserEmail = "";
        static string currentRole = "";

        const string carsFile = "cars.csv";
        const string usersFile = "users.csv";
        const string ordersFile = "orders.csv";
        const string managerPass = "123";

        static void Main()
        {
            LoadAllData();
            LoginFlow();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"--- {currentRole.ToUpper()} MENU | {currentUserEmail} ---");
                PrintMenu();

                Console.Write("Select option: ");
                string choice = Console.ReadLine();

                switch (currentRole)
                {
                    case "Manager": HandleManager(choice, ref exit); break;
                    case "Seller": HandleSeller(choice, ref exit); break;
                    case "Client": HandleClient(choice, ref exit); break;
                }
            }
        }

        static void PrintMenu()
        {
            if (currentRole == "Manager")
                Console.WriteLine("1. Show All Cars 2. Approve Pending Cars 3. Sales History 4. Remove Car 5. Stats 6. Exit");
            else if (currentRole == "Seller")
                Console.WriteLine("1. Show Approved Cars 2. Add Car (Wait for Approval) 3. All Sales History 4. Exit");
            else
                Console.WriteLine("1. Show Cars 2. Buy Car 3. My Purchase History 4. Search/Filter 5. Exit");
        }

        #region Role Handlers
        static void HandleManager(string c, ref bool e)
        {
            switch (c)
            {
                case "1": ShowCars(null); break;
                case "2": ApproveCars(); break;
                case "3": ShowHistory(false); break;
                case "4": RemoveCar(); break;
                case "5": ShowStats(); break;
                case "6": e = true; break;
            }
        }

        static void HandleSeller(string c, ref bool e)
        {
            switch (c)
            {
                case "1": ShowCars(true); break;
                case "2": AddCar(false); break;
                case "3": ShowHistory(false); break;
                case "4": e = true; break;
            }
        }

        static void HandleClient(string c, ref bool e)
        {
            switch (c)
            {
                case "1": ShowCars(true); break;
                case "2": BuyCar(); break;
                case "3": ShowHistory(true); break;
                case "4": AdvancedFilter(); break;
                case "5": e = true; break;
            }
        }
        #endregion

        #region Core Logic
        static void ShowCars(bool? approvedOnly)
        {
            var list = approvedOnly.HasValue
                ? cars.Where(c => c.IsApproved == approvedOnly.Value)
                : cars;

            PrintCarTable(list);
            Console.WriteLine("Press any key..."); Console.ReadKey();
        }

        static void PrintCarTable(IEnumerable<Car> list)
        {
            Console.WriteLine("{0,-5} | {1,-20} | {2,-10} | {3,-6} | {4,-10}", "ID", "Name", "Price", "Year", "Approved");
            Console.WriteLine(new string('-', 60));
            foreach (var c in list)
                Console.WriteLine("{0,-5} | {1,-20} | {2,-10}$ | {3,-6} | {4,-10}", c.Id, c.Name, c.Price, c.Year, c.IsApproved);
        }

        static void AdvancedFilter()
        {
            Console.Clear();
            Console.WriteLine("--- ADVANCED FILTER ---");
            Console.Write("Brand (leave empty for all): ");
            string brand = Console.ReadLine().ToLower();

            Console.Write("Min Price: ");
            double.TryParse(Console.ReadLine(), out double minPrice);

            Console.Write("Max Price (leave 0 for no limit): ");
            double.TryParse(Console.ReadLine(), out double maxPrice);

            Console.Write("Min Year: ");
            int.TryParse(Console.ReadLine(), out int minYear);

            var query = cars.Where(c => c.IsApproved);

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(c => c.Name.ToLower().Contains(brand));

            if (minPrice > 0)
                query = query.Where(c => c.Price >= minPrice);

            if (maxPrice > 0)
                query = query.Where(c => c.Price <= maxPrice);

            if (minYear > 0)
                query = query.Where(c => c.Year >= minYear);

            var results = query.ToList();

            if (results.Any())
            {
                Console.WriteLine("--- SEARCH RESULTS ---");
                PrintCarTable(results);
            }
            else
            {
                Console.WriteLine("No cars found matching criteria.");
            }
            Console.ReadKey();
        }

        static void AddCar(bool autoApprove)
        {
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("Price: "); double price = double.Parse(Console.ReadLine());
            Console.Write("Year: "); int year = int.Parse(Console.ReadLine());
            Console.Write("Type: "); string type = Console.ReadLine();

            int id = cars.Count == 0 ? 1 : cars.Max(c => c.Id) + 1;
            cars.Add(new Car(id, name, price, year, type, autoApprove));
            SaveCars();
            Console.WriteLine(autoApprove ? "Added!" : "Sent to Manager for approval.");
            Console.ReadKey();
        }

        static void ApproveCars()
        {
            var pending = cars.Where(c => !c.IsApproved).ToList();
            if (!pending.Any()) { Console.WriteLine("Nothing to approve."); Console.ReadKey(); return; }

            foreach (var car in pending)
            {
                Console.WriteLine($"Approve {car.Name} ({car.Price}$)? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    int idx = cars.FindIndex(c => c.Id == car.Id);
                    var updated = cars[idx]; updated.IsApproved = true; cars[idx] = updated;
                }
            }
            SaveCars();
        }

        static void BuyCar()
        {
            Console.Write("Enter Car ID to buy: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            var car = cars.FirstOrDefault(c => c.Id == id && c.IsApproved);
            if (car.Name == null) { Console.WriteLine("Not found."); Console.ReadKey(); return; }

            orders.Add(new Order(currentUserEmail, car.Name, car.Price, DateTime.Now));
            cars.RemoveAll(c => c.Id == id);

            SaveCars();
            SaveOrders();
            Console.WriteLine($"Bought {car.Name}! 12-month warranty active.");
            Console.ReadKey();
        }

        static void ShowHistory(bool mineOnly)
        {
            var list = mineOnly ? orders.Where(o => o.UserEmail == currentUserEmail) : orders;
            Console.WriteLine("--- SALES HISTORY ---");
            foreach (var o in list)
                Console.WriteLine($"{o.SaleDate:dd/MM/yy} | {o.UserEmail} | {o.CarName} | {o.Price}$");
            Console.WriteLine("Press any key..."); Console.ReadKey();
        }

        static void RemoveCar()
        {
            Console.Write("ID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                cars.RemoveAll(c => c.Id == id);
                SaveCars();
            }
        }

        static void ShowStats()
        {
            if (!cars.Any()) return;
            Console.WriteLine($"Total: {cars.Count} | Avg Price: {cars.Average(c => c.Price):F2}$");
            Console.ReadKey();
        }
        #endregion

        #region Auth & Data
        static void LoginFlow()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1-Client | 2-Seller | 3-Manager");
                string roleChoice = Console.ReadLine();

                if (roleChoice == "3")
                {
                    Console.Write("Password: ");
                    if (Console.ReadLine() == managerPass)
                    {
                        currentRole = "Manager"; currentUserEmail = "admin"; return;
                    }
                }
                else
                {
                    currentRole = roleChoice == "2" ? "Seller" : "Client";
                    AuthUser(); return;
                }
            }
        }

        static void AuthUser()
        {
            while (true)
            {
                Console.WriteLine("1-Login 2-Register");
                string cmd = Console.ReadLine();
                if (cmd == "1" && Login()) return;
                if (cmd == "2") Register();
            }
        }

        static bool Login()
        {
            Console.Write("Email: "); string e = Console.ReadLine();
            Console.Write("Pass: "); string p = ComputeHash(Console.ReadLine());
            if (users.Any(u => u.Email == e && u.PasswordHash == p))
            {
                currentUserEmail = e; return true;
            }
            return false;
        }

        static void Register()
        {
            Console.Write("Email: "); string e = Console.ReadLine();
            Console.Write("Pass: "); string p = Console.ReadLine();
            int id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;
            users.Add(new User(id, e, ComputeHash(p)));
            SaveUsers();
        }

        static string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        static void LoadAllData()
        {
            if (File.Exists(carsFile)) cars = File.ReadAllLines(carsFile).Skip(1).Select(l => Car.FromCsv(l)).Where(x => x.HasValue).Select(x => x.Value).ToList();
            if (File.Exists(usersFile)) users = File.ReadAllLines(usersFile).Skip(1).Select(l => User.FromCsv(l)).Where(x => x.HasValue).Select(x => x.Value).ToList();
            if (File.Exists(ordersFile)) orders = File.ReadAllLines(ordersFile).Skip(1).Select(l => Order.FromCsv(l)).Where(x => x.HasValue).Select(x => x.Value).ToList();
        }

        static void SaveCars() => File.WriteAllLines(carsFile, new[] { "Id,Name,Price,Year,Type,IsApproved" }.Concat(cars.Select(c => c.ToString())));
        static void SaveUsers() => File.WriteAllLines(usersFile, new[] { "Id,Email,PasswordHash" }.Concat(users.Select(u => u.ToString())));
        static void SaveOrders() => File.WriteAllLines(ordersFile, new[] { "Email,Car,Price,Date" }.Concat(orders.Select(o => o.ToString())));
        #endregion
    }
}