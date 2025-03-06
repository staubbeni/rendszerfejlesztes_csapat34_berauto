using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerAuto.Services
{
    public interface ICarCategoryService
    {
        List<CarCategory> GetAllCarCategories();
    }

    public class CarCategoryService : ICarCategoryService
    {
        private readonly AppDbContext _context;

        public CarCategoryService(AppDbContext context)
        {
            _context = context;
        }
        public List<CarCategory> List()
        {
            return _context.CarCategories.ToList();
        }
    }

}
