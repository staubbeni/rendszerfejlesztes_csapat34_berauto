using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Context;
using Microsoft.EntityFrameworkCore;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IAddressService _addressService;
        private readonly AppDbContext _context;

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
                string? guestAddress = null;
                string? guestEmail = null;
                string? guestName = null;
                string? guestPhone = null;

                if (User?.Identity != null && User.Identity.IsAuthenticated)
                {
                    var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (idClaim != null && int.TryParse(idClaim.Value, out var parsedId))
                    {
                        userId = parsedId;
                    }
                }

                if (userId.HasValue) // Bejelentkezett felhasználó
                {
                    var user = await _context.Users
                        .Include(u => u.Address)
                        .FirstOrDefaultAsync(u => u.Id == userId.Value);
                    if (user == null)
                    {
                        return BadRequest("User not found.");
                    }

                    // Cím lekérdezése az adatbázisból
                    var userAddress = await _addressService.GetAddressByUserIdAsync(userId.Value);
                    if (userAddress == null && string.IsNullOrWhiteSpace(dto.GuestAddress))
                    {
                        return BadRequest("User address is required. Please update your profile with an address or provide one in the request.");
                    }

                    // Ha a DTO-ban van megadva cím, azt használjuk; különben az adatbázisból
                    guestAddress = !string.IsNullOrWhiteSpace(dto.GuestAddress)
                        ? dto.GuestAddress
                        : $"{userAddress.City}, {userAddress.Street}, {userAddress.ZipCode}, {userAddress.State}";
                    guestEmail = !string.IsNullOrWhiteSpace(dto.GuestEmail) ? dto.GuestEmail : user.Email;
                    guestName = !string.IsNullOrWhiteSpace(dto.GuestName) ? dto.GuestName : user.Name;
                    guestPhone = !string.IsNullOrWhiteSpace(dto.GuestPhone) ? dto.GuestPhone : user.PhoneNumber;
                }
                else // Vendég
                {
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

                // Ellenőrizzük, hogy a GuestEmail és GuestAddress nem üres
                if (string.IsNullOrWhiteSpace(guestEmail))
                {
                    return BadRequest("Guest email cannot be empty.");
                }
                if (string.IsNullOrWhiteSpace(guestAddress))
                {
                    return BadRequest("Guest address cannot be empty.");
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

        // További metódusok változatlanok
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