using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Services
{
    public interface IRentalService
    {
        Task<Rental> CreateRentalAsync(Rental rental);
        Task<List<Rental>> GetUserRentalsAsync(int userId);
        Task<Rental> GetRentalByIdAsync(int id);
        Task UpdateRentalStatusAsync(int rentalId, string status);
    }
}


namespace BerAuto.Services
{
    public class RentalService : IRentalService
    {
        private readonly AppDbContext _context;

        public RentalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rental> CreateRentalAsync(Rental rental)
        {
            var car = await _context.Cars.FindAsync(rental.CarId);
            if (car != null && car.IsAvailable)
            {
                car.IsAvailable = false;
                rental.TotalCost = CalculateTotalCost(rental.StartDate, rental.EndDate, car.DailyRate);
                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();
                return rental;
            }
            throw new Exception("Car is not available");
        }

        public async Task<List<Rental>> GetUserRentalsAsync(int userId)
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<Rental> GetRentalByIdAsync(int id)
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateRentalStatusAsync(int rentalId, string status)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);
            if (rental != null)
            {
                rental.Status = status;
                if (status == "Completed" || status == "Cancelled")
                {
                    var car = await _context.Cars.FindAsync(rental.CarId);
                    if (car != null)
                    {
                        car.IsAvailable = true;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        private int CalculateTotalCost(DateTime start, DateTime end, int dailyRate)
        {
            return (int)(end - start).TotalDays * dailyRate;
        }
    }
}