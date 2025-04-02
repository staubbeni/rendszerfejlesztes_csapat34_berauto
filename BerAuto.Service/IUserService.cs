using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
    }
}


namespace BerAuto.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Rentals)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
        }
    }
}