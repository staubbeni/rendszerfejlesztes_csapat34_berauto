using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerAuto.Models;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Data
{
    public class RentalService
    {
        private readonly BerAutoContext _context;
        
        public RentalService(BerAutoContext context)
        {
            _context = context;
        }
        
        public async Task<RentalRequest> CreateRentalRequest(RentalRequest request)
        {
            // Ellenőrizzük, hogy az autó elérhető-e a kért időszakban
            var isCarAvailable = await IsCarAvailableForPeriod(request.CarId, request.StartDate, request.EndDate);
            
            if (!isCarAvailable)
            {
                throw new Exception("Az autó nem elérhető a kiválasztott időszakban!");
            }
            
            request.RequestDate = DateTime.Now;
            request.Status = "Pending";
            
            _context.RentalRequests.Add(request);
            await _context.SaveChangesAsync();
            
            return request;
        }
        
        public async Task<List<RentalRequest>> GetPendingRequests()
        {
            return await _context.RentalRequests
                .Include(r => r.User)
                .Include(r => r.Car)
                .Where(r => r.Status == "Pending")
                .ToListAsync();
        }
        
        public async Task<RentalRequest> ApproveRentalRequest(int requestId)
        {
            var request = await _context.RentalRequests.FindAsync(requestId);
            
            if (request == null)
            {
                throw new Exception("A kölcsönzési kérelem nem található!");
            }
            
            // Ellenőrizzük, hogy az autó még mindig elérhető-e
            var isCarAvailable = await IsCarAvailableForPeriod(request.CarId, request.StartDate, request.EndDate);
            
            if (!isCarAvailable)
            {
                throw new Exception("Az autó már nem elérhető a kiválasztott időszakban!");
            }
            
            request.Status = "Approved";
            
            // Létrehozzuk a kölcsönzést
            var car = await _context.Cars.FindAsync(request.CarId);
            
            var rental = new Rental
            {
                UserId = request.UserId,
                CarId = request.CarId,
                RentalRequestId = request.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                StartMileage = car.Mileage,
                IsReturned = false,
                Status = "Active",
                TotalCost = CalculateRentalCost(car.DailyRate, request.StartDate, request.EndDate)
            };
            
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            
            return request;
        }
        
        public async Task<RentalRequest> RejectRentalRequest(int requestId, string reason)
        {
            var request = await _context.RentalRequests.FindAsync(requestId);
            
            if (request == null)
            {
                throw new Exception("A kölcsönzési kérelem nem található!");
            }
            
            request.Status = "Rejected";
            request.Notes = reason;
            
            await _context.SaveChangesAsync();
            
            return request;
        }
        
        public async Task<Rental> HandOverCar(int rentalId)
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == rentalId);
                
            if (rental == null)
            {
                throw new Exception("A kölcsönzés nem található!");
            }
            
            // Az autó már nem elérhető
            var car = rental.Car;
            car.IsAvailable = false;
            
            await _context.SaveChangesAsync();
            
            return rental;
        }
        
        public async Task<Rental> ReturnCar(int rentalId, int endMileage)
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == rentalId);
                
            if (rental == null)
            {
                throw new Exception("A kölcsönzés nem található!");
            }
            
            rental.IsReturned = true;
            rental.ActualReturnDate = DateTime.Now;
            rental.EndMileage = endMileage;
            rental.Status = "Completed";
            
            // Frissítjük az autó kilométeróra állását és elérhetőségét
            var car = rental.Car;
            car.Mileage = endMileage;
            car.IsAvailable = true;
            
            // Ha túllépte a kölcsönzési időt, újraszámoljuk a költséget
            if (rental.ActualReturnDate > rental.EndDate)
            {
                rental.TotalCost = CalculateRentalCost(car.DailyRate, rental.StartDate, rental.ActualReturnDate.Value);
            }
            
            await _context.SaveChangesAsync();
            
            return rental;
        }
        
        public async Task<Invoice> CreateInvoice(int rentalId)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);
            
            if (rental == null)
            {
                throw new Exception("A kölcsönzés nem található!");
            }
            
            var invoice = new Invoice
            {
                RentalId = rentalId,
                IssueDate = DateTime.Now,
                Amount = rental.TotalCost,
                IsPaid = false
            };
            
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            
            return invoice;
        }
        
        public async Task<List<Rental>> GetUserRentals(int userId)
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        
        public async Task<List<Rental>> GetActiveRentals()
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Car)
                .Where(r => r.Status == "Active")
                .ToListAsync();
        }
        
        public async Task<List<Rental>> GetOverdueRentals()
        {
            var today = DateTime.Today;
            
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Car)
                .Where(r => r.Status == "Active" && r.EndDate < today)
                .ToListAsync();
        }
        
        private async Task<bool> IsCarAvailableForPeriod(int carId, DateTime startDate, DateTime endDate)
        {
            // Ellenőrizzük, hogy az autó alapvetően elérhető-e
            var car = await _context.Cars.FindAsync(carId);
            
            if (car == null || !car.IsAvailable)
            {
                return false;
            }
            
            // Ellenőrizzük, hogy nincs-e átfedés más kölcsönzésekkel
            var overlappingRentals = await _context.Rentals
                .Where(r => r.CarId == carId && r.Status != "Completed" &&
                       (startDate <= r.EndDate && endDate >= r.StartDate))
                .AnyAsync();
                
            return !overlappingRentals;
        }
        
        private decimal CalculateRentalCost(decimal dailyRate, DateTime startDate, DateTime endDate)
        {
            var days = (int)(endDate - startDate).TotalDays + 1;
            return dailyRate * days;
        }

        public async Task<List<Rental>> GetAllRentals()
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Car)
                .ToListAsync();
        }

        public async Task<Rental> GetRentalById(int id)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateRental(Rental rental)
        {
            // Ellenőrizzük, hogy az autó elérhető-e
            var car = await _context.Cars.FindAsync(rental.CarId);
            if (car == null || !car.IsAvailable)
            {
                throw new Exception("Az autó nem elérhető!");
            }
            
            // Beállítjuk az autót foglaltra
            car.IsAvailable = false;
            
            // Mentjük a foglalást
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
        }

        public async Task CompleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                throw new Exception("A foglalás nem található!");
            }
            
            // Beállítjuk a visszahozatal dátumát
            rental.ReturnDate = DateTime.Now;
            
            // Kiszámoljuk a végösszeget
            var car = await _context.Cars.FindAsync(rental.CarId);
            var days = (rental.ReturnDate.Value - rental.RentalDate).Days;
            if (days == 0) days = 1; // Minimum 1 nap
            
            rental.TotalAmount = days * car.DailyRate;
            
            // Beállítjuk az autót elérhetőre
            car.IsAvailable = true;
            
            await _context.SaveChangesAsync();
        }

        public async Task CancelRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                throw new Exception("A foglalás nem található!");
            }
            
            // Beállítjuk az autót elérhetőre
            var car = await _context.Cars.FindAsync(rental.CarId);
            car.IsAvailable = true;
            
            // Töröljük a foglalást
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
        }
    }
} 