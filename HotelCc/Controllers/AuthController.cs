using HotelCc.Data;
using HotelCc.Models;
using HotelCc.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

        // GET: Login
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }


        // POST: Login
        [HttpPost]
        public IActionResult Login(
            LoginViewModel model,
            string? returnUrl)
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

            HttpContext.Session.SetInt32(
                "UserId",
                user.Id);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

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


        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // =========================
            // VALIDAR EMAIL DUPLICADO
            // =========================
            var existe = await _context.Usuarios
                .AnyAsync(u => u.Email == model.Email);

            if (existe)
            {
                ModelState.AddModelError("", "El email ya existe");

                return View(model);
            }


            Usuario user = new Usuario()
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Password = model.Password,

                // SIEMPRE USER
                Rol = "User"
            };

            _context.Usuarios.Add(user);

            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
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