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
        Task<Address> AddAddressAsync(Address address);
        Task<Address> GetAddressByUserIdAsync(int userId);
        Task UpdateAddressAsync(Address address);
    }
}


namespace BerAuto.Services
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;

        public AddressService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Address> AddAddressAsync(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> GetAddressByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }
    }
}
