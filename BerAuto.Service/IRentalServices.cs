using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BerAuto.DataContext.Context;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BerAuto.DataContext.Context;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Entities;
using Microsoft.EntityFrameworkCore;


namespace BerAuto.Services
{
    public interface IRentalService
    {
        // Customer or Guest: create rental request
        Task<RentalDto> RequestRentalAsync(int? userId, RentalRequestDto req);

        // Customer: view own rental history
        Task<IEnumerable<RentalDto>> GetUserRentalsAsync(int userId);

        // Employee/Admin: view all rentals
        Task<IEnumerable<RentalDto>> GetAllRentalsAsync();

        // Employee/Admin: approve or reject rental request
        Task<bool> ApproveRentalAsync(int rentalId);
        Task<bool> RejectRentalAsync(int rentalId);

        // Employee/Admin: record pickup and return events
        Task<bool> RecordPickupAsync(int rentalId);
        Task<bool> RecordReturnAsync(int rentalId);

        // Employee/Admin: generate invoice PDF
        Task<byte[]> GenerateInvoiceAsync(int rentalId);
    }
}
namespace BerAuto.Services
{
    public class RentalService : IRentalService
    {
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;

        public RentalService(AppDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<RentalDto> RequestRentalAsync(int? userId, RentalRequestDto req)
        {
            var rental = new Rental
            {
                UserId = userId,
                GuestName = userId == null ? req.GuestName : null,
                GuestEmail = userId == null ? req.GuestEmail : null,
                GuestPhone = userId == null ? req.GuestPhone : null,
                GuestAddress = userId == null ? req.GuestAddress : null,
                CarId = req.CarId,
                RequestDate = DateTime.UtcNow,
                Status = RentalStatus.Pending,
                TotalCost = 0m // compute later or in another step
            };
            _ctx.Rentals.Add(rental);
            await _ctx.SaveChangesAsync();
            return _mapper.Map<RentalDto>(rental);
        }

        public async Task<IEnumerable<RentalDto>> GetUserRentalsAsync(int userId)
        {
            var list = await _ctx.Rentals
                .Include(r => r.Car)
                .Where(r => r.UserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<RentalDto>>(list);
        }

        public async Task<IEnumerable<RentalDto>> GetAllRentalsAsync()
        {
            var list = await _ctx.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .ToListAsync();
            return _mapper.Map<IEnumerable<RentalDto>>(list);
        }

        public async Task<bool> ApproveRentalAsync(int rentalId)
        {
            var r = await _ctx.Rentals.FindAsync(rentalId);
            if (r == null) return false;
            r.Status = RentalStatus.Approved;
            r.ApprovalDate = DateTime.UtcNow;
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectRentalAsync(int rentalId)
        {
            var r = await _ctx.Rentals.FindAsync(rentalId);
            if (r == null) return false;
            r.Status = RentalStatus.Rejected;
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RecordPickupAsync(int rentalId)
        {
            var r = await _ctx.Rentals.FindAsync(rentalId);
            if (r == null || r.Status != RentalStatus.Approved) return false;
            r.Status = RentalStatus.PickedUp;
            r.PickupDate = DateTime.UtcNow;
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RecordReturnAsync(int rentalId)
        {
            var r = await _ctx.Rentals.FindAsync(rentalId);
            if (r == null || r.Status != RentalStatus.PickedUp) return false;
            r.Status = RentalStatus.Returned;
            r.ReturnDate = DateTime.UtcNow;
            await _ctx.SaveChangesAsync();
            return true;
        }

        public Task<byte[]> GenerateInvoiceAsync(int rentalId)
        {
            // PDF generation stub
            throw new NotImplementedException();
        }
    }
}
