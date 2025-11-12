using System;

namespace CarShowroom
{
    internal class Program
    {
        static bool hasPurchase = false;
        static string lastCar = "";
        static int lastQuantity = 0;
        static int lastDiscount = 0;
        static double lastTax = 0;
        static double lastFinalPriceWithTax = 0;

        static void Main(string[] args)
        {
            bool exit = false;

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

            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("WELCOME TO CARSHOWROOM");
                Console.ResetColor();
                Console.WriteLine("1. Show Car List");
                Console.WriteLine("2. Buy a Car");
                if (hasPurchase)
                    Console.WriteLine("3. Purchase Receipt");
                Console.WriteLine(hasPurchase ? "4. Settings" : "3. Settings");
                Console.WriteLine(hasPurchase ? "5. Info about cars" : "4. Info about cars");
                Console.WriteLine(hasPurchase ? "6. Exit" : "5. Exit");

                Console.Write("Please select an option: ");
                int choice = 0;

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid number entered!");
                    Console.ResetColor();
                    continue;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.ResetColor();
                    continue;
                }
                finally
                {
                    Console.WriteLine("Input processing is complete.");
                }

                switch (choice)
                {
                    case 1:
                        ShowCarList();
                        break;
                    case 2:
                        BuyCar(carsName, carsPrice);
                        break;
                    case 3:
                        if (hasPurchase)
                            ShowReceipt();
                        else
                            Settings();
                        break;
                    case 4:
                        if (hasPurchase)
                            Settings();
                        else
                            InfoAboutCars();
                        break;
                    case 5:
                        if (hasPurchase)
                            InfoAboutCars();
                        else
                            exit = true;
                        break;
                    case 6:
                        if (hasPurchase)
                            exit = true;
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

            static void Settings()
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Settings Menu (not implemented)");
                Console.ResetColor();
                Console.WriteLine("Press any key to return to main menu...");
                Console.ReadKey();
                Console.Clear();
            }

            static void InfoAboutCars()
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Information About Cars (not implemented)");
                Console.ResetColor();
                Console.WriteLine("Press any key to return to main menu...");
                Console.ReadKey();
                Console.Clear();
            }

            static void ShowCarList()
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("List of cars:");
                Console.ResetColor();

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

                for (int i = 0; i < carsName.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{i + 1}. {carsName[i]} - ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{carsPrice[i]:N0}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("$");
                    Console.ResetColor();
                }
            }

            static void BuyCar(string[] carsName, double[] carsPrice)
            {
                Console.Clear();
                ShowCarList();

                int choosCar = 0;
                try
                {
                    Console.Write("Enter the number of the car you want to buy (1-5) or 0 to return: ");
                    choosCar = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: You must enter only numbers!");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                finally
                {
                    Console.WriteLine("Input verification complete.");
                }

                if (choosCar == 0)
                {
                    Console.WriteLine("Press Enter...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                if (choosCar < 1 || choosCar > carsName.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid car number. Returning to main menu...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                choosCar -= 1;

                int quantity = 0;
                try
                {
                    Console.Write("Enter quantity: ");
                    quantity = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: You must enter only numbers!");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                double totalPrice = carsPrice[choosCar] * quantity;
                Random random = new Random();
                int discount = random.Next(1, 10);
                double finalPrice = totalPrice - (totalPrice * discount / 100);
                finalPrice = Math.Round(finalPrice, 2);

                double taxRate = Math.Pow(finalPrice / 100000, 0.2) * 0.02;
                double tax = finalPrice * taxRate;
                tax = Math.Round(tax, 2);

                double finalPriceWithTax = finalPrice + tax;

                hasPurchase = true;
                lastCar = carsName[choosCar];
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
                Console.Clear();
            }
        }
    }
}
