using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Context; // Hozz�adva
using Microsoft.EntityFrameworkCore; // Hozz�adva

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IAddressService _addressService;
        private readonly AppDbContext _context; // Hozz�adva

        public RentalController(IRentalService rentalService, IAddressService addressService, AppDbContext context)
        {
            _rentalService = rentalService;
            _addressService = addressService;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RequestRental([FromBody] RentalRequestDto dto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int? userId = null;
                if (User?.Identity != null && User.Identity.IsAuthenticated)
                {
                    var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (idClaim != null && int.TryParse(idClaim.Value, out var parsedId))
                    {
                        userId = parsedId;
                    }
                }

                string? guestAddress = null;
                string? guestEmail = null;
                string? guestName = null;
                string? guestPhone = null;

                if (userId.HasValue) // Bejelentkezett felhasználó
                {
                    // Felhasználó adatainak lekérése
                    var user = await _context.Users
                        .Include(u => u.Address)
                        .FirstOrDefaultAsync(u => u.Id == userId.Value);
                    if (user == null)
                    {
                        return BadRequest("User not found.");
                    }

                    // Cím ellenőrzése
                    var userAddress = await _addressService.GetAddressByUserIdAsync(userId.Value);
                    if (userAddress == null)
                    {
                        return BadRequest("User address is required. Please update your profile with an address first.");
                    }

                    // Felhasználó adatainak kitöltése
                    guestAddress = $"{userAddress.City}, {userAddress.Street}, {userAddress.ZipCode}, {userAddress.State}";
                    guestEmail = user.Email; // Felhasználó email címe
                    guestName = user.Name; // Opcionális: felhasználó neve
                    guestPhone = user.PhoneNumber; // Opcionális: felhasználó telefonszáma
                }
                else // Vendég
                {
                    // Validáljuk a vendég adatait
                    if (string.IsNullOrWhiteSpace(dto.GuestName) ||
                        string.IsNullOrWhiteSpace(dto.GuestEmail) ||
                        string.IsNullOrWhiteSpace(dto.GuestPhone) ||
                        string.IsNullOrWhiteSpace(dto.GuestAddress))
                    {
                        return BadRequest("All guest information (name, email, phone, address) is required.");
                    }
                    guestAddress = dto.GuestAddress;
                    guestEmail = dto.GuestEmail;
                    guestName = dto.GuestName;
                    guestPhone = dto.GuestPhone;
                }

                // Ellenőrizzük, hogy a GuestEmail nem üres
                if (string.IsNullOrWhiteSpace(guestEmail))
                {
                    return BadRequest("Guest email cannot be empty.");
                }

                // Módosítjuk a DTO-t
                dto.GuestAddress = guestAddress;
                dto.GuestEmail = guestEmail;
                dto.GuestName = guestName;
                dto.GuestPhone = guestPhone;

                var rental = await _rentalService.RequestRentalAsync(userId, dto);
                return Ok(rental);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // HTTP 409
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unexpected error: " + ex.Message);
            }
        }

        // Tov�bbi met�dusok v�ltozatlanok
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyRentals()
        {
            int? userId = null;
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null && int.TryParse(idClaim.Value, out var parsedId))
                {
                    userId = parsedId;
                }
            }
            if (!userId.HasValue)
            {
                return Unauthorized();
            }
            var list = await _rentalService.GetUserRentalsAsync(userId.Value);
            return Ok(list);
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> List()
            => Ok(await _rentalService.GetAllRentalsAsync());

        [HttpPost("Approve/{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Approve(int id)
            => (await _rentalService.ApproveRentalAsync(id)) ? Ok() : NotFound();

        [HttpPost("Reject/{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Reject(int id)
            => (await _rentalService.RejectRentalAsync(id)) ? Ok() : NotFound();

        [HttpPost("Pickup/{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Pickup(int id)
            => (await _rentalService.RecordPickupAsync(id)) ? Ok() : NotFound();

        [HttpPost("Return/{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Return(int id)
            => (await _rentalService.RecordReturnAsync(id)) ? Ok() : NotFound();

        [HttpGet("Invoice/{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Invoice(int id)
        {
            var pdf = await _rentalService.GenerateInvoiceAsync(id);
            return File(pdf, "application/pdf", $"Invoice_{id}.pdf");
        }
    }
}