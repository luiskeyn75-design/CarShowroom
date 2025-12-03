using System;
using System.Collections.Generic;
using System.Text;

namespace CarShowroom
{
    internal class Program
    {
        struct Car
        {
            public int id;
            public string CarName;
            public double Price;
            public int Year;
            public string Type;

            public Car(int id, string name, double price, int year, string type)
            {
                this.id = id;
                CarName = name;
                Price = price;
                Year = year;
                Type = type;
            }
        }

        static List<Car> cars = new List<Car>();
        static Dictionary<string, string> accounts = new Dictionary<string, string>();
        static bool isManager = false;
        static readonly string managerPassword = "1234";
        static Random random = new Random();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string[] carsName = { "Chevrolet Corvette C7 Z06", "Dodge RAM TRX 1500", "Chevrolet Corvette C8 Z06", "Audi RS6 Performance GT", "Audi Q8" };
            double[] carsPrice = { 100000, 150000, 150000, 238000, 97000 };
            int[] carsYear = { 2016, 2021, 2023, 2024, 2022 };
            string[] carsType = { "Sport", "Truck", "Sport", "Sport", "SUV" };

            for (int i = 0; i < carsName.Length; i++)
                cars.Add(new Car(random.Next(1000, 9999), carsName[i], carsPrice[i], carsYear[i], carsType[i]));

            RoleSelect();
            MainMenu();
        }

        static void RoleSelect()
        {
            while (true)
            {
                Console.WriteLine("Select your role:");
                Console.WriteLine("1 - Client");
                Console.WriteLine("2 - Manager");
                Console.Write("Choice: ");
                string cmd = Console.ReadLine();

                if (cmd == "1") { ClientAuth(); return; }
                else if (cmd == "2")
                {
                    Console.Write("Enter manager password: ");
                    string p = ReadPassword();
                    if (p == managerPassword) { isManager = true; Console.WriteLine("Manager access granted."); return; }
                    else { Console.WriteLine("Wrong password."); continue; }
                }
                else Console.WriteLine("Invalid.");
            }
        }

        static void ClientAuth()
        {
            while (true)
            {
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Register");
                Console.Write("Choice: ");
                string cmd = Console.ReadLine();

                if (cmd == "1")
                {
                    if (Login()) return;
                    else Environment.Exit(0);
                }
                else if (cmd == "2") Register();
                else Console.WriteLine("Invalid.");
            }
        }

        static bool Login()
        {
            int attempts = 0;
            while (attempts < 3)
            {
                Console.Write("Enter username: ");
                string u = Console.ReadLine();
                Console.Write("Enter password: ");
                string p = ReadPassword();

                if (accounts.ContainsKey(u) && accounts[u] == p)
                {
                    Console.WriteLine("Login successful.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Wrong data.");
                    attempts++;
                }
            }
            return false;
        }

        static void Register()
        {
            Console.Write("Enter username: ");
            string u = Console.ReadLine();
            if (accounts.ContainsKey(u)) { Console.WriteLine("User exists."); return; }
            Console.Write("Enter password: ");
            string p = ReadPassword();
            accounts[u] = p;
            Console.WriteLine("Registration complete.");
        }

        static string ReadPassword()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
                if (key.Key == ConsoleKey.Backspace && sb.Length > 0) { sb.Length--; Console.Write("\b \b"); continue; }
                sb.Append(key.KeyChar);
                Console.Write("*");
            }
            return sb.ToString();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("WELCOME TO CARSHOWROOM");
                Console.WriteLine("1. Show Car List");
                Console.WriteLine("2. Add Cars");
                Console.WriteLine("3. Search Car by ID");
                Console.WriteLine("4. Delete Car by ID");
                Console.WriteLine("5. Sort Cars by Price (Bubble Sort)");
                Console.WriteLine("6. Sort Cars by Price (List.Sort())");
                Console.WriteLine("7. Show Statistics");
                Console.WriteLine("8. Exit");
                Console.Write("Select option: ");
                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

                switch (choice)
                {
                    case 1: ShowCarList(); break;
                    case 2: AddCarsInteractive(); break;
                    case 3: SearchCarByID(); break;
                    case 4: DeleteCarByID(); break;
                    case 5: BubbleSortCarsByPrice(); break;
                    case 6: ListSortCarsByPrice(); break;
                    case 7: ShowStatistics(); break;
                    case 8: return;
                }
            }
        }

        static void ShowCarList()
        {
            Console.Clear();
            Console.WriteLine("{0,-6} {1,-30} {2,-10} {3,-6} {4,-10}", "ID", "Name", "Price", "Year", "Type");
            Console.WriteLine(new string('-', 70));
            for (int i = 0; i < cars.Count; i++)
            {
                Car c = cars[i];
                Console.WriteLine("{0,-6} {1,-30} {2,-10} {3,-6} {4,-10}", c.id, c.CarName, c.Price, c.Year, c.Type);
            }
            Console.ReadKey();
        }

        static void AddCarsInteractive()
        {
            Console.Clear();
            Console.Write("How many cars to add: ");
            if (!int.TryParse(Console.ReadLine(), out int cnt) || cnt < 1) return;

            for (int i = 0; i < cnt; i++)
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();
                Console.Write("Price: ");
                if (!double.TryParse(Console.ReadLine(), out double price)) { Console.WriteLine("Invalid price."); i--; continue; }
                Console.Write("Year: ");
                if (!int.TryParse(Console.ReadLine(), out int year)) { Console.WriteLine("Invalid year."); i--; continue; }
                Console.Write("Type: ");
                string type = Console.ReadLine();
                cars.Add(new Car(random.Next(1000, 9999), name, price, year, type));
            }
        }

        static void SearchCarByID()
        {
            Console.Clear();
            Console.Write("Enter ID to search: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID."); Console.ReadKey(); return; }

            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].id == id)
                {
                    Car c = cars[i];
                    Console.WriteLine("Found car:");
                    Console.WriteLine("{0,-6} {1,-30} {2,-10} {3,-6} {4,-10}", c.id, c.CarName, c.Price, c.Year, c.Type);
                    Console.ReadKey();
                    return;
                }
            }
            Console.WriteLine("Car not found.");
            Console.ReadKey();
        }

        static void DeleteCarByID()
        {
            Console.Clear();
            Console.Write("Enter ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID."); Console.ReadKey(); return; }

            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].id == id)
                {
                    cars.RemoveAt(i);
                    Console.WriteLine("Car deleted.");
                    Console.ReadKey();
                    return;
                }
            }
            Console.WriteLine("Car not found.");
            Console.ReadKey();
        }

        static void BubbleSortCarsByPrice()
        {
            for (int i = 0; i < cars.Count - 1; i++)
            {
                for (int j = 0; j < cars.Count - i - 1; j++)
                {
                    if (cars[j].Price > cars[j + 1].Price)
                    {
                        Car temp = cars[j];
                        cars[j] = cars[j + 1];
                        cars[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine("Cars sorted by price (Bubble Sort).");
            Console.ReadKey();
        }

        static void ListSortCarsByPrice()
        {
            cars.Sort((x, y) => x.Price.CompareTo(y.Price));
            Console.WriteLine("Cars sorted by price (List.Sort()).");
            Console.ReadKey();
        }

        static void ShowStatistics()
        {
            Console.Clear();
            if (cars.Count == 0) { Console.WriteLine("No cars available."); Console.ReadKey(); return; }

            double sum = 0;
            double minPrice = cars[0].Price;
            double maxPrice = cars[0].Price;

            for (int i = 0; i < cars.Count; i++)
            {
                sum += cars[i].Price;
                if (cars[i].Price < minPrice) minPrice = cars[i].Price;
                if (cars[i].Price > maxPrice) maxPrice = cars[i].Price;
            }

            double avg = sum / cars.Count;

            Console.WriteLine("Total cars: " + cars.Count);
            Console.WriteLine("Min price: " + minPrice);
            Console.WriteLine("Max price: " + maxPrice);
            Console.WriteLine("Sum of prices: " + sum);
            Console.WriteLine("Average price: " + avg);
            Console.ReadKey();
        }
    }
}
