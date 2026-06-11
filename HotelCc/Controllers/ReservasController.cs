using HotelCc.Data;
using HotelCc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelCc.Controllers
{
    public class ReservasController : Controller
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX
        // =========================
        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (rol == "Admin")
            {
                var reservasAdmin = await _context.Reservas
                    .Include(r => r.Usuario)
                    .Include(r => r.Habitacion)
                    .OrderByDescending(r => r.Id)
                    .ToListAsync();

                return View(reservasAdmin);
            }

            var reservasUser = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .Where(r => r.UsuarioId == userId)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            return View(reservasUser);
        }

        // =========================
        // RESERVAR DESDE HABITACIÓN
        // =========================
        public async Task<IActionResult> Reservar(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth",
                    new
                    {
                        returnUrl = Url.Action(
                            "Reservar",
                            "Reservas",
                            new { id = id })
                    });
            }

            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion == null)
                return NotFound();

            var rol = HttpContext.Session.GetString("Rol");

            ViewBag.EsAdmin = (rol == "Admin");

            // SOLO USERS
            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios
                    .Where(u =>
                        u.Rol != null &&
                        u.Rol.Trim().ToLower() == "user")
                    .OrderBy(u => u.Nombre),
                "Id",
                "Nombre"
            );

            var model = new Reserva
            {
                HabitacionId = id,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(1)
            };

            ViewBag.Habitacion = habitacion;

            return View(model);
        }

        // =========================
        // POST RESERVAR
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar(Reserva reserva)
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            var rol = HttpContext.Session.GetString("Rol");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            // =========================
            // VALIDAR FECHAS
            // =========================
            if (reserva.FechaSalida <= reserva.FechaEntrada)
            {
                ViewBag.Error = "La fecha de salida debe ser mayor.";

                return View(reserva);
            }

            // =========================
            // ADMIN / USER
            // =========================
            if (rol == "Admin")
            {
                if (reserva.UsuarioId == null &&
                    string.IsNullOrWhiteSpace(reserva.NombreHuespedExterno))
                {
                    ViewBag.Error =
                        "Debe seleccionar un usuario o ingresar huésped externo.";

                    return View(reserva);
                }

                if (reserva.UsuarioId != null)
                {
                    reserva.NombreHuespedExterno = null;
                }
                else
                {
                    reserva.UsuarioId = null;
                }
            }
            else
            {
                reserva.UsuarioId = userId;
                reserva.NombreHuespedExterno = null;
            }

            // =========================
            // VALIDAR DISPONIBILIDAD
            // =========================
            bool ocupada = await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == reserva.HabitacionId &&
                reserva.FechaEntrada < r.FechaSalida &&
                reserva.FechaSalida > r.FechaEntrada);

            if (ocupada)
            {
                ViewBag.Error = "La habitación ya está reservada para esa fecha.";

                return View(reserva);
            }

            // =========================
            // HABITACIÓN
            // =========================
            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            if (habitacion == null)
            {
                return NotFound();
            }

            // =========================
            // TOTAL
            // =========================
            int dias = (reserva.FechaSalida - reserva.FechaEntrada).Days;

            if (dias <= 0)
            {
                dias = 1;
            }

            reserva.Total = dias * habitacion.Precio;

            // =========================
            // FECHA RESERVA
            // =========================
            reserva.FechaReserva = DateTime.Now;

            _context.Reservas.Add(reserva);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // CREATE ADMIN
        // =========================
        public IActionResult Create()
        {
            var usuarios = _context.Usuarios
                .Where(u =>
                    u.Rol != null &&
                    u.Rol.Trim().ToLower() == "user")
                .OrderBy(u => u.Nombre)
                .ToList();

            ViewData["UsuarioId"] =
                new SelectList(usuarios, "Id", "Nombre");

            ViewData["HabitacionId"] =
                new SelectList(
                    _context.Habitaciones.OrderBy(h => h.Numero),
                    "Id",
                    "Numero"
                );

            return View();
        }

        // =========================
        // POST CREATE
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {


            // =========================
            // HABITACIÓN
            // =========================
            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            if (habitacion == null)
            {
                return NotFound();
            }

            // =========================
            // FECHAS
            // =========================
            var fechaEntrada = reserva.FechaEntrada.Date;
            var fechaSalida = reserva.FechaSalida.Date;

            if (fechaSalida <= fechaEntrada)
            {
                ViewBag.Error = "La fecha de salida debe ser mayor.";

                return View(reserva);
            }

            // =========================
            // USER / EXTERNO
            // =========================
            if (reserva.UsuarioId != null)
            {
                reserva.NombreHuespedExterno = null;
            }
            else
            {
                reserva.UsuarioId = null;
            }

            // =========================
            // DISPONIBILIDAD
            // =========================
            bool ocupada = await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == reserva.HabitacionId &&
                fechaEntrada < r.FechaSalida &&
                fechaSalida > r.FechaEntrada);

            if (ocupada)
            {
                ViewBag.Error = "La habitación ya está reservada.";

                return View(reserva);
            }

            // =========================
            // TOTAL
            // =========================
            int dias = (fechaSalida - fechaEntrada).Days;

            if (dias <= 0)
            {
                dias = 1;
            }

            decimal total = dias * habitacion.Precio;

            // =========================
            // NUEVA RESERVA
            // =========================
            var nuevaReserva = new Reserva
            {
                UsuarioId = reserva.UsuarioId,

                NombreHuespedExterno =
                    reserva.NombreHuespedExterno,

                HabitacionId = reserva.HabitacionId,

                FechaEntrada = fechaEntrada,

                FechaSalida = fechaSalida,

                FechaReserva = DateTime.Now,

                Total = total
            };

            _context.Reservas.Add(nuevaReserva);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EXISTS
        // =========================
        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}