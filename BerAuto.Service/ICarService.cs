using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
using BerAuto.DataContext.Dtos;

namespace BerAuto.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetAllCarsAsync();
        Task<CarDto> GetCarByIdAsync(int id);
        Task<CarDto> CreateCarAsync(CarDto carDto);
        Task<CarDto> UpdateCarAsync(int id, CarDto carDto);
        Task<bool> DeleteCarAsync(int id);
        Task<bool> SetAvailabilityAsync(int id, bool available);
        Task<bool> UpdateOdometerAsync(int id, int newReading);
    }

    public class CarService : ICarService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CarService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
        {
            var cars = await _context.Cars.ToListAsync();
            return _mapper.Map<IEnumerable<CarDto>>(cars);
        }

        public async Task<CarDto> GetCarByIdAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            return car == null ? null : _mapper.Map<CarDto>(car);
        }

        public async Task<CarDto> CreateCarAsync(CarDto carDto)
        {
            var car = _mapper.Map<Car>(carDto);
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return _mapper.Map<CarDto>(car);
        }

        public async Task<CarDto> UpdateCarAsync(int id, CarDto carDto)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return null;
            _mapper.Map(carDto, car);
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return _mapper.Map<CarDto>(car);
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetAvailabilityAsync(int id, bool available)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;
            car.IsAvailable = available;
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOdometerAsync(int id, int newReading)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;
            car.Odometer = newReading;
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}