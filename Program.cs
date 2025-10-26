using System;

namespace CarShowroom
{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("WELCOME TO CARSHOWROOM");
            Console.ResetColor();

            Console.WriteLine("Press any key to see a list of cars");
            Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("List of cars:");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("1. Chevrolet Corvette C7 Z06 - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("100 000");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("$");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("2. Dodge RAM TRX 1500 - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("150 000");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("$");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("3. Chevrolet Corvette C8 Z06 - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("150 000");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("$");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("4. Audi RS6 Performance GT - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("238 000");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("$");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("5. Audi Q8 - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("97 000");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("$");
            Console.ResetColor();

            Console.Write("Enter the number of the car you want to buy (1-5): ");
            int choosCar = Convert.ToInt32(Console.ReadLine()) - 1;

            Console.Write("Enter quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            double totalPrice = carsPrice[choosCar] * quantity;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Total Price: " + totalPrice + "$");
            Console.ResetColor();

            Random random = new Random();
            int discount = random.Next(1, 10);
            double finalPrice = totalPrice - (totalPrice * discount / 100);
            finalPrice = Math.Round(finalPrice, 2);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Congratulations! You got a discount of " + discount + "%");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Final Price after discount: " + finalPrice + "$");
            Console.ResetColor();

            double taxRate = Math.Pow(finalPrice / 100000, 0.2) * 0.02;
            double tax = finalPrice * taxRate;
            tax = Math.Round(tax, 2);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Calculated luxury tax: " + tax + "$");
            Console.ResetColor();

            double finalPriceWithTax = finalPrice + tax;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Final Price (with tax): " + finalPriceWithTax + "$");
            Console.ResetColor();

            Console.WriteLine("Press any key to confirm your purchase...");
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Purchase confirmed!");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Your order details:");
            Console.ResetColor();
            Console.WriteLine("Car: " + carsName[choosCar]);
            Console.WriteLine("Quantity: " + quantity);
            Console.WriteLine("Discount: " + discount + "%");
            Console.WriteLine("Tax: " + tax + "$");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Final Price (with tax): " + finalPriceWithTax + "$");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Thank you for shopping at CarShowroom!");
            Console.ResetColor();
        }
    }
}
