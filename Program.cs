using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarShowroom
{
    internal class Program
    {
        struct Car
        {
            public string CarName;   
            public double Price;     
            public int Year;         
            public string Type;     

            public Car(string name, double price, int year, string type)
            {
                CarName = name;
                Price = price;
                Year = year;
                Type = type;
            }
        }

        static List<Car> cars = new List<Car>();
        static bool hasPurchase = false;
        static string lastCar = "";
        static int lastQuantity = 0;
        static int lastDiscount = 0;
        static double lastTax = 0;
        static double lastFinalPriceWithTax = 0;

        static Dictionary<string, string> accounts = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string[] carsName =
            {
                "Chevrolet Corvette C7 Z06",
                "Dodge RAM TRX 1500",
                "Chevrolet Corvette C8 Z06",
                "Audi RS6 Performance GT",
                "Audi Q8"
            };

            double[] carsPrice =
            {
                100000,
                150000,
                150000,
                238000,
                97000
            };

            int[] carsYear = { 2016, 2021, 2023, 2024, 2022 };
            string[] carsType = { "Sport", "Truck", "Sport", "Sport", "SUV" };

            for (int i = 0; i < carsName.Length; i++)
            {
                cars.Add(new Car(carsName[i], carsPrice[i], carsYear[i], carsType[i]));
            }

            Console.WriteLine("Welcome. You need to Login to continue or Register a new account.");
            AuthMenu();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("WELCOME TO CARSHOWROOM");
                Console.ResetColor();
                Console.WriteLine("1. Show Car List");
                Console.WriteLine("2. Buy a Car");
                Console.WriteLine("3. Add Cars (enter multiple)");
                Console.WriteLine("4. Statistics");
                if (hasPurchase) Console.WriteLine("5. Purchase Receipt");
                Console.WriteLine(hasPurchase ? "6. Settings" : "5. Settings");
                Console.WriteLine(hasPurchase ? "7. Info about cars" : "6. Info about cars");
                Console.WriteLine(hasPurchase ? "8. Exit" : "7. Exit");
                Console.Write("Please select an option: ");

                int choice;
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        throw new FormatException("Must enter a number.");
                    }
                }
                catch (FormatException fe)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + fe.Message);
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue; 
                }

                switch (choice)
                {
                    case 1:
                        ShowCarList();
                        break;
                    case 2:
                        BuyCar();
                        break;
                    case 3:
                        AddCarsInteractive();
                        break;
                    case 4:
                        ShowStatistics();
                        break;
                    case 5:
                        if (hasPurchase) ShowReceipt(); else Settings();
                        break;
                    case 6:
                        if (hasPurchase) Settings(); else InfoAboutCars();
                        break;
                    case 7:
                        if (hasPurchase) InfoAboutCars(); else exit = true;
                        break;
                    case 8:
                        if (hasPurchase) exit = true;
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ResetColor();
                        }
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ResetColor();
                        break;
                }
            }

            Console.WriteLine("Goodbye.");
        }

        static void AuthMenu()
        {
            while (true)
            {
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Register");
                Console.Write("Choice: ");
                string cmd = Console.ReadLine();
                if (cmd == "1")
                {
                    bool ok = Login();
                    if (ok) return;
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Failed to login. Exiting application.");
                        Console.ResetColor();
                        Environment.Exit(0);
                    }
                }
                else if (cmd == "2")
                {
                    Register();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Unknown command. Try again.");
                    Console.ResetColor();
                }
            }
        }

        static bool Login()
        {
            const int maxAttempts = 3;
            int attempts = 0;
            do
            {
                Console.Write("Enter username (email): ");
                string username = Console.ReadLine()?.Trim();
                Console.Write("Enter password: ");
                string password = ReadPassword();

                if (username == null || password == null)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    attempts++;
                    continue; 
                }

                if (accounts.ContainsKey(username) && accounts[username] == password)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Login successful.");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Login failed.");
                    Console.ResetColor();
                    attempts++;
                }
            } while (attempts < maxAttempts);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Maximum attempts ({maxAttempts}) reached.");
            Console.ResetColor();
            return false;
        }

        static void Register()
        {
            Console.Write("Enter new username (email): ");
            string username = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Invalid username. Returning to auth menu.");
                return;
            }

            if (accounts.ContainsKey(username))
            {
                Console.WriteLine("User already exists. Try logging in.");
                return;
            }

            Console.Write("Enter new password: ");
            string password = ReadPassword();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Invalid password. Returning to auth menu.");
                return;
            }

            accounts[username] = password;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registration successful. Now login.");
            Console.ResetColor();
        }

        static string ReadPassword()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            return sb.ToString();
        }

        static void ShowCarList()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("List of cars:");
            Console.ResetColor();

            for (int i = 0; i < cars.Count; i++)
            {
                var c = cars[i];
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{i + 1}. {c.CarName} - ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{c.Price:N0}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("$");
                Console.ResetColor();
                Console.WriteLine($"    Year: {c.Year}, Type: {c.Type}");
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        static void BuyCar()
        {
            Console.Clear();
            ShowCarList();

            Console.Write("Enter the number of the car you want to buy (1-{0}) or 0 to return: ", cars.Count);
            int chooseCar;
            if (!int.TryParse(Console.ReadLine(), out chooseCar))
            {
                Console.WriteLine("Error: You must enter only numbers!");
                Console.ReadKey();
                return;
            }

            if (chooseCar == 0) return;

            if (chooseCar < 1 || chooseCar > cars.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid car number. Returning to main menu...");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            chooseCar -= 1; 
            int quantity;
            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.WriteLine("Error: Quantity must be a positive integer!");
                Console.ReadKey();
                return;
            }

            double totalPrice = cars[chooseCar].Price * quantity;
            Random random = new Random();
            int discount = random.Next(1, 10); 
            double finalPrice = totalPrice - (totalPrice * discount / 100.0);
            finalPrice = Math.Round(finalPrice, 2);

            double taxRate = Math.Pow(finalPrice / 100000.0, 0.2) * 0.02;
            double tax = finalPrice * taxRate;
            tax = Math.Round(tax, 2);

            double finalPriceWithTax = finalPrice + tax;

            hasPurchase = true;
            lastCar = cars[chooseCar].CarName;
            lastQuantity = quantity;
            lastDiscount = discount;
            lastTax = tax;
            lastFinalPriceWithTax = finalPriceWithTax;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Purchase confirmed!");
            Console.ResetColor();
            ShowReceipt();
        }

        static void ShowReceipt()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Your Purchase Receipt:");
            Console.ResetColor();
            Console.WriteLine($"Car: {lastCar}");
            Console.WriteLine($"Quantity: {lastQuantity}");
            Console.WriteLine($"Discount: {lastDiscount}%");
            Console.WriteLine($"Tax: {lastTax}$");
            Console.WriteLine($"Final Price (with tax): {lastFinalPriceWithTax}$");

            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
        }

        static void Settings()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Settings Menu (not implemented)");
            Console.ResetColor();
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
        }

        static void InfoAboutCars()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Information About Cars (not implemented)");
            Console.ResetColor();
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
        }

        static void AddCarsInteractive()
        {
            Console.Clear();
            Console.Write("How many cars would you like to add? (min 1): ");
            int count;
            if (!int.TryParse(Console.ReadLine(), out count) || count < 1)
            {
                Console.WriteLine("Invalid number. Returning to menu.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Entering car #{i + 1}:");
                Console.Write("Name: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Empty name — skipping this entry.");
                    continue; 
                }

                Console.Write("Price: ");
                if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
                {
                    Console.WriteLine("Invalid price — skipping this entry.");
                    continue;
                }

                Console.Write("Year: ");
                if (!int.TryParse(Console.ReadLine(), out int year) || year < 1886 || year > DateTime.Now.Year + 1)
                {
                    Console.WriteLine("Invalid year — skipping this entry.");
                    continue;
                }

                Console.Write("Type: ");
                string type = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(type))
                {
                    type = "Unknown";
                }

                cars.Add(new Car(name, price, year, type));
                Console.WriteLine("Car added.");
            }

            Console.WriteLine("Adding complete. Press any key...");
            Console.ReadKey();
        }

        static void ShowStatistics()
        {
            Console.Clear();
            if (cars.Count == 0)
            {
                Console.WriteLine("No cars in database.");
                Console.ReadKey();
                return;
            }

            double total = 0;
            double min = double.MaxValue;
            double max = double.MinValue;
            int countPriceAbove = 0;
            double threshold;
            Console.Write("Enter price threshold to count cars above (e.g., 100000): ");
            if (!double.TryParse(Console.ReadLine(), out threshold))
            {
                threshold = 100000; 
            }

            foreach (var c in cars)
            {
                if (c.Price < 0) continue; 
                if (c.Price > 10_000_000) 
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Extremely high price detected, stopping scan early.");
                    Console.ResetColor();
                    break; 
                }

                total += c.Price;
                if (c.Price < min) min = c.Price;
                if (c.Price > max) max = c.Price;
                if (c.Price > threshold) countPriceAbove++;
            }

            int n = cars.Count;
            double average = n > 0 ? total / n : 0;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("STATISTICS REPORT");
            Console.ResetColor();
            Console.WriteLine($"Total number of car records: {n}");
            Console.WriteLine($"Total price sum: {total:N2}$");
            Console.WriteLine($"Average price: {average:N2}$");
            Console.WriteLine($"Count of cars with price > {threshold:N2}$: {countPriceAbove}");
            Console.WriteLine($"Minimum price: {(min == double.MaxValue ? 0 : min):N2}$");
            Console.WriteLine($"Maximum price: {(max == double.MinValue ? 0 : max):N2}$");

            PrintFormattedReport(total, average, countPriceAbove, min, max);

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        static void PrintFormattedReport(double total, double avg, int countAbove, double min, double max)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("===== FORMATTED REPORT =====");
            Console.ResetColor();
            Console.WriteLine($"Generated at: {DateTime.Now}");
            Console.WriteLine($"Total sum: {total:N2}$");
            Console.WriteLine($"Average: {avg:N2}$");
            Console.WriteLine($"Count above threshold: {countAbove}");
            Console.WriteLine($"Min price: {(min == double.MaxValue ? 0 : min):N2}$");
            Console.WriteLine($"Max price: {(max == double.MinValue ? 0 : max):N2}$");
            Console.WriteLine("List of top 3 most expensive cars:");
            var top3 = cars.OrderByDescending(c => c.Price).Take(3).ToList();
            for (int i = 0; i < top3.Count; i++)
            {
                var c = top3[i];
                Console.WriteLine($"{i + 1}. {c.CarName} — {c.Price:N2}$ ({c.Year}, {c.Type})");
            }
            Console.WriteLine("============================");
        }
    }
}
