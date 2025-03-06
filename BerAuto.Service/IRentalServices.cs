using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerAuto.Services
{
    public interface IRentalServices
    {
        List<Rental> GetAllRental();
    }

    public class RentalService : IRentalServices
    {
        private readonly AppDbContext _context;
        public RentalService(AppDbContext context)
        {
            _context = context;
        }
        public List<Rental> List()
        {
            return _context.GetAllRental.ToList();
        }
    } 
}
