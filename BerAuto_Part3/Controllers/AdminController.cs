using BerAuto.Data;
using BerAuto.Models;
using Microsoft.AspNetCore.Mvc;

namespace BerAuto.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly CarService _carService;

        public AdminController(UserService userService, CarService carService)
        {
            _userService = userService;
            _carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            // Itt később ellenőrizni kell, hogy a felhasználó admin-e
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> Cars()
        {
            var cars = await _carService.GetAllCars();
            return View(cars);
        }
    }
} 