using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Entities;
using BerAuto.DataContext.Context;
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
        Task<Address> CreateAddressAsync(AddressDto addressDto, int userId);
        Task<Address> GetAddressByUserIdAsync(int userId);
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

        public async Task<Address> CreateAddressAsync(AddressDto addressDto, int userId)
        {
            // Ellenőrizzük, van-e már címe a felhasználónak
            var existingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (existingAddress != null)
            {
                // Frissítsük a meglévő címet
                existingAddress.Street = addressDto.Street;
                existingAddress.City = addressDto.City;
                existingAddress.State = addressDto.State;
                existingAddress.ZipCode = addressDto.ZipCode;

                await _context.SaveChangesAsync();
                return existingAddress;
            }
            else
            {
                // Új cím létrehozása
                var address = new Address
                {
                    Street = addressDto.Street,
                    City = addressDto.City,
                    State = addressDto.State,
                    ZipCode = addressDto.ZipCode,
                    UserId = userId
                };

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                return address;
            }
        }

        public async Task<Address> GetAddressByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }
    }
}