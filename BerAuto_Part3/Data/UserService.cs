using System;
using System.Linq;
using System.Threading.Tasks;
using BerAuto.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BerAuto.Data
{
    public class UserService
    {
        private readonly BerAutoContext _context;
        
        public UserService(BerAutoContext context)
        {
            _context = context;
        }
        
        public async Task<User> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Felhasználónév és jelszó megadása kötelező!");
            }
            
            var hashedPassword = HashPassword(password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);
                
            if (user == null)
            {
                throw new Exception("Hibás felhasználónév vagy jelszó!");
            }
            
            return user;
        }
        
        public async Task Register(User user, string password)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Felhasználónév és jelszó megadása kötelező!");
            }
            
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                throw new Exception("A felhasználónév már foglalt!");
            }
            
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new Exception("Az e-mail cím már foglalt!");
            }
            
            user.UserType = "User"; // Alapértelmezetten sima felhasználó
            user.RegistrationDate = DateTime.Now;
            
            user.Password = HashPassword(password);
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
        
        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        
        public async Task UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                
                return builder.ToString();
            }
        }
    }
} 