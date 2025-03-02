using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerAuto.Service
{
   public interface ICarService
    {
        List<Car> GetAllCars();
    }
    public class CarService : ICarService
    {
        private readonly AppDbContext _context;

        public CarService(AppDbContext context)
        {
            _context = context;
        }

        public List<Car> List()
        {
            return _context.Cars.ToList();
        }
    }
}

