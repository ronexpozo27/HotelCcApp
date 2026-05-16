using HotelCc.Data;
using HotelCc.Models;
using HotelCc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelCc.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var usuarios = _context.Usuarios.ToList();

            var user = usuarios.FirstOrDefault(u =>
                u.Email!.Trim().ToLower() ==
                model.Email!.Trim().ToLower());

            if (user == null)
            {
                ViewBag.Error = "Usuario no encontrado";

                return View(model);
            }

            if (user.Password!.Trim() != model.Password!.Trim())
            {
                ViewBag.Error = "Contraseña incorrecta";

                return View(model);
            }

            HttpContext.Session.SetString(
                "Usuario",
                user.Nombre ?? "");

            HttpContext.Session.SetString(
                "Rol",
                user.Rol ?? "");

            // DEBUG TEMPORAL
            Console.WriteLine("ROL = " + user.Rol);

            if (user.Rol!.Trim() == "Admin")
            {
                return RedirectToAction(
                    "Dashboard",
                    "Admin");
            }

            return RedirectToAction(
                "Index",
                "Reservas");
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Auth");
        }
    }
}