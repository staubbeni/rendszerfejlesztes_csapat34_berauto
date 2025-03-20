using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerAuto.Services
{
    public interface IAddressService
    {
        List<Address> GetAllAddress();
    }
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;


        public AddressService(AppDbContext context)
        {
            _context = context;
        }
        public List<Address> GetAllAddress()
        {
            return _context.Addresses.ToList();
        }
    }
}
