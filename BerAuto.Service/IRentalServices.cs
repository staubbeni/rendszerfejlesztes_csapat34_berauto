using AutoMapper;
using BerAuto.DataContext.Context;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using System.Threading.Tasks;

namespace BerAuto.Services
{
    public interface IRentalService
    {
        Task<RentalDto> RequestRentalAsync(int? userId, RentalRequestDto req);
        Task<IEnumerable<RentalDto>> GetUserRentalsAsync(int userId);
        Task<IEnumerable<RentalDto>> GetAllRentalsAsync();
        Task<bool> ApproveRentalAsync(int rentalId);
        Task<bool> RejectRentalAsync(int rentalId);
        Task<bool> RecordPickupAsync(int rentalId);
        Task<bool> RecordReturnAsync(int rentalId);
        Task<byte[]> GenerateInvoiceAsync(int rentalId);
    }

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
            // Validációk
            if (req == null)
                throw new ArgumentNullException(nameof(req));

            // Autó létezésének és elérhetőségének ellenőrzése
            var car = await _ctx.Cars.FirstOrDefaultAsync(c => c.Id == req.CarId);
            if (car == null)
                throw new ArgumentException($"Car with ID {req.CarId} not found.");
            if (!car.IsAvailable)
                throw new InvalidOperationException("Car is not available for rental.");

            // Időintervallum validálása
            if (req.To <= req.From)
                throw new ArgumentException("End date must be after start date.");

            // Autó foglaltságának ellenőrzése
            var isCarRented = await _ctx.Rentals
                .AnyAsync(r => r.CarId == req.CarId &&
                               r.Status != RentalStatus.Rejected &&
                               r.Status != RentalStatus.Returned &&
                               (req.From <= r.To && req.To >= r.From));
            if (isCarRented)
                throw new InvalidOperationException("Car is already rented for the specified period.");

            // GuestAddress és GuestEmail ellenőrzése
            if (string.IsNullOrWhiteSpace(req.GuestAddress))
                throw new ArgumentException("Guest address is required.");
            if (string.IsNullOrWhiteSpace(req.GuestEmail))
                throw new ArgumentException("Guest email is required.");

            // Rental entitás létrehozása
            var rental = new Rental
            {
                UserId = userId,
                GuestName = userId == null ? req.GuestName : req.GuestName, // Mindig kitöltve
                GuestEmail = req.GuestEmail, // Mindig kitöltve
                GuestPhone = userId == null ? req.GuestPhone : req.GuestPhone, // Mindig kitöltve
                GuestAddress = req.GuestAddress, // Mindig kitöltve
                CarId = req.CarId,
                RequestDate = DateTime.UtcNow,
                Status = RentalStatus.Pending,
                From = req.From,
                To = req.To
            };

            // Költség kiszámítása
            var rentalDays = (req.To - req.From).Days;
            if (rentalDays <= 0)
                rentalDays = 1; // Minimum 1 nap
            rental.TotalCost = rentalDays * car.DailyRate;

            // Mentés az adatbázisba
            await _ctx.Rentals.AddAsync(rental);
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

        public async Task<byte[]> GenerateInvoiceAsync(int rentalId)
        {
            var rental = await _ctx.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == rentalId);
            if (rental == null)
                throw new ArgumentException("Rental not found.");

            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12);

                gfx.DrawString($"Számla #{rental.Id}", font, XBrushes.Black, new XRect(50, 50, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Autó: {rental.Car.Brand} {rental.Car.Model}", font, XBrushes.Black, new XRect(50, 70, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Költség: {rental.TotalCost} Ft", font, XBrushes.Black, new XRect(50, 90, page.Width, page.Height), XStringFormats.TopLeft);

                using (var stream = new MemoryStream())
                {
                    document.Save(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}