using BerAuto.Data;
using BerAuto.Models;
using Microsoft.AspNetCore.Mvc;

namespace BerAuto.Controllers
{
    public class AssistantController : Controller
    {
        private readonly RentalService _rentalService;

        public AssistantController(RentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<IActionResult> Index()
        {
            // Itt később ellenőrizni kell, hogy a felhasználó ügyintéző-e
            return View();
        }

        public async Task<IActionResult> Rentals()
        {
            var rentals = await _rentalService.GetAllRentals();
            return View(rentals);
        }
    }
} 