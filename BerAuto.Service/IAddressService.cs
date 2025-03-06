using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
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
        private readonly AppDbcontext _addressService;
        public AddressService(AppDbContext context)
        {
            _context = context;
        }
        public List<Address> List()
        {
            return _context.Address.ToList();
        }
    }
}
