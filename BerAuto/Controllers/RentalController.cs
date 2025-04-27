using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BerAuto.Services;
using BerAuto.DataContext.Dtos;

namespace BerAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Request([FromBody] RentalRequestDto dto)
        {
            int? userId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                : (int?)null;

            if (!userId.HasValue) // vendég
            {
                if (string.IsNullOrWhiteSpace(dto.GuestName)
                    || string.IsNullOrWhiteSpace(dto.GuestEmail)
                    || string.IsNullOrWhiteSpace(dto.GuestPhone)
                    || string.IsNullOrWhiteSpace(dto.GuestAddress))
                    return BadRequest("Guest information required.");
            }

            var rental = await _rentalService.RequestRentalAsync(userId, dto);
            return Ok(rental);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyRentals()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var list = await _rentalService.GetUserRentalsAsync(userId);
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
