using System;
using System.Linq;
using BerAuto.Models;

namespace BerAuto.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BerAutoContext context)
        {
            // Mivel az adatbázis már létezik, nem kell létrehozni
            // Csak ellenőrizzük, hogy elérhető-e
            if (context.Database.CanConnect())
            {
                Console.WriteLine("Adatbázis kapcsolat sikeres.");
            }
            else
            {
                Console.WriteLine("Nem sikerült kapcsolódni az adatbázishoz!");
            }
        }
        
        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                
                var builder = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                
                return builder.ToString();
            }
        }
    }
} 