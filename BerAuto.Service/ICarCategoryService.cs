using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Services
{
    public interface ICarCategoryService
    {
        Task<List<CarCategory>> GetAllCategoriesAsync();
        Task<CarCategory> GetCategoryByIdAsync(int id);
        Task<CarCategory> AddCategoryAsync(CarCategory category);
        Task UpdateCategoryAsync(CarCategory category);
    }
}


namespace BerAuto.Services
{
    public class CarCategoryService : ICarCategoryService
    {
        private readonly AppDbContext _context;

        public CarCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarCategory>> GetAllCategoriesAsync()
        {
            return await _context.CarCategories.ToListAsync();
        }

        public async Task<CarCategory> GetCategoryByIdAsync(int id)
        {
            return await _context.CarCategories
                .Include(c => c.Cars)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CarCategory> AddCategoryAsync(CarCategory category)
        {
            _context.CarCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategoryAsync(CarCategory category)
        {
            _context.CarCategories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}