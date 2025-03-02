using BerAuto.Data;
using BerAuto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BerAuto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CarService _carService;

        public HomeController(ILogger<HomeController> logger, CarService carService)
        {
            _logger = logger;
            _carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var cars = await _carService.GetAvailableCars();
                return View(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hiba történt az autók lekérdezése során");
                return View(new List<Car>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 