using System;

namespace CarShowroom
{
    public struct User
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
}