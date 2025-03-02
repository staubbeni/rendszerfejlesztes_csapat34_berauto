using BerAuto.Data;
using BerAuto.Models;
using Microsoft.AspNetCore.Mvc;

namespace BerAuto.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userService.Login(model.Username, model.Password);
                    
                    // Itt kellene a felhasználói munkamenet kezelése
                    // Egyszerűsítés kedvéért most csak átirányítunk
                    
                    switch (user.UserType)
                    {
                        case "Admin":
                            return RedirectToAction("Index", "Admin");
                        case "Assistant":
                            return RedirectToAction("Index", "Assistant");
                        default:
                            return RedirectToAction("Index", "User");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User
                    {
                        Username = model.Username,
                        FullName = model.FullName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address
                    };
                    
                    await _userService.Register(user, model.Password);
                    
                    TempData["SuccessMessage"] = "Sikeres regisztráció! Most már bejelentkezhet.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            
            return View(model);
        }
    }
} 