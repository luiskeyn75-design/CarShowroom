using System;

namespace CarShowroom
{
    public struct Order
    {
        public string UserEmail;
        public string CarName;
        public double Price;
        public DateTime SaleDate;

        public Order(string email, string carName, double price, DateTime date)
        {
            UserEmail = email;
            CarName = carName;
            Price = price;
            SaleDate = date;
        }

        public override string ToString() => $"{UserEmail},{CarName},{Price},{SaleDate:o}";

        public static Order? FromCsv(string line)
        {
            try
            {
                var parts = line.Split(',');
                if (parts.Length != 4) return null;
                return new Order(parts[0], parts[1], double.Parse(parts[2]), DateTime.Parse(parts[3]));
            }
            catch { return null; }
        }
    }
}