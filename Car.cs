using System;

namespace CarShowroom
{
    public struct Car
    {
        public int Id;
        public string Name;
        public double Price;
        public int Year;
        public string Type;
        public bool IsApproved; 

        public Car(int id, string name, double price, int year, string type, bool isApproved = true)
        {
            Id = id;
            Name = name;
            Price = price;
            Year = year;
            Type = type;
            IsApproved = isApproved;
        }

        public override string ToString() => $"{Id},{Name},{Price},{Year},{Type},{IsApproved}";

        public static Car? FromCsv(string line)
        {
            try
            {
                var parts = line.Split(',');
                if (parts.Length != 6) return null;
                return new Car(
                    int.Parse(parts[0]),
                    parts[1],
                    double.Parse(parts[2]),
                    int.Parse(parts[3]),
                    parts[4],
                    bool.Parse(parts[5])
                );
            }
            catch { return null; }
        }
    }
}