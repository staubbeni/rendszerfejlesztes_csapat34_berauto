using BerAuto.Data;
using BerAuto.Models;
using Microsoft.AspNetCore.Mvc;

namespace BerAuto.Controllers
{
    public class UserController : Controller
    {
        private readonly CarService _carService;
        private readonly RentalService _rentalService;

        public UserController(CarService carService, RentalService rentalService)
        {
            _carService = carService;
            _rentalService = rentalService;
        }

        public IActionResult Index()
        {
            // Itt később ellenőrizni kell, hogy a felhasználó be van-e jelentkezve
            return View();
        }

        public async Task<IActionResult> Cars()
        {
            var cars = await _carService.GetAvailableCars();
            return View(cars);
        }

        public async Task<IActionResult> MyRentals(int userId)
        {
            // Itt később a bejelentkezett felhasználó ID-ját kell használni
            var rentals = await _rentalService.GetUserRentals(userId);
            return View(rentals);
        }
    }
} 