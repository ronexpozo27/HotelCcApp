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


        // =========================
        // POST: LOGIN
        // =========================
        [HttpPost]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl)
        {
            // =========================
            // VALIDAR CAMPOS
            // =========================

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ViewBag.Error = "Ingrese su correo electrónico.";
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Error = "Ingrese su contraseña.";
                return View(model);
            }

            // =========================
            // BUSCAR USUARIO
            // =========================

            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Email != null &&
                    u.Email.Trim().ToLower() ==
                    model.Email.Trim().ToLower());

            // =========================
            // VALIDAR USUARIO
            // =========================

            if (user == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View(model);
            }

            // =========================
            // VALIDAR CONTRASEÑA
            // =========================

            if (string.IsNullOrWhiteSpace(user.Password) ||
                user.Password.Trim() != model.Password.Trim())
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View(model);
            }

            // =========================
            // CREAR SESIÓN
            // =========================

            HttpContext.Session.SetString(
                "Usuario",
                user.Nombre ?? "");

            HttpContext.Session.SetString(
                "Rol",
                user.Rol ?? "");

            HttpContext.Session.SetInt32(
                "UserId",
                user.Id);

            // =========================
            // REDIRECCIONAR
            // =========================

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (user.Rol == "Admin")
            {
                return RedirectToAction(
                    "Dashboard",
                    "Admin");
            }

            return RedirectToAction(
                "Index",
                "Habitaciones");
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