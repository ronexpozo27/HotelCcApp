using Microsoft.AspNetCore.Mvc;
using HotelCc.Data;
using HotelCc.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HotelCc.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Email == model.Email &&
                    u.Password == model.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("Usuario", user.Nombre ?? "");
                HttpContext.Session.SetString("Rol", user.Rol ?? "");

                return RedirectToAction("Index", "Habitaciones");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
