using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerAuto.Models;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Data
{
    public class CarService
    {
        private readonly BerAutoContext _context;
        
        public CarService(BerAutoContext context)
        {
            _context = context;
        }
        
        public async Task<List<Car>> GetAvailableCars()
        {
            return await _context.Cars.Where(c => c.IsAvailable).ToListAsync();
        }
        
        public async Task<Car> GetCarById(int id)
        {
            return await _context.Cars.FindAsync(id);
        }
        
        public async Task<List<Car>> GetAllCars()
        {
            return await _context.Cars.ToListAsync();
        }
        
        public async Task AddCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateCar(Car car)
        {
            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task UpdateMileage(int carId, int newMileage)
        {
            var car = await _context.Cars.FindAsync(carId);
            
            if (car == null)
            {
                throw new Exception("Az autó nem található!");
            }
            
            car.Mileage = newMileage;
            await _context.SaveChangesAsync();
        }
        
        public async Task SetAvailability(int carId, bool isAvailable)
        {
            var car = await _context.Cars.FindAsync(carId);
            
            if (car == null)
            {
                throw new Exception("Az autó nem található!");
            }
            
            car.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
        }
    }
} 