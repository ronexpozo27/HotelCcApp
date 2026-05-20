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
        // RESERVAR DESDE TARJETA
        // =========================

        public async Task<IActionResult> Reservar(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion == null)
            {
                return NotFound();
            }

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

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // VALIDACIÓN FECHAS

            if (reserva.FechaSalida <= reserva.FechaEntrada)
            {
                ViewBag.Error = "La fecha de salida debe ser mayor.";

                return View(reserva);
            }

            // VALIDAR DISPONIBILIDAD

            var fechaEntrada = reserva.FechaEntrada.Date;

            var fechaSalida = reserva.FechaSalida.Date;

            // VALIDAR FECHAS PASADAS

            if (fechaEntrada < DateTime.Today)
            {
                ViewBag.Error = "No puedes reservar fechas pasadas.";

                return View(reserva);
            }


            bool ocupada = await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == reserva.HabitacionId &&
                fechaEntrada < r.FechaSalida &&
                fechaSalida > r.FechaEntrada);

            if (ocupada)
            {
                ViewBag.Error = "La habitación ya está reservada en esas fechas.";

                return View(reserva);
            }

            // HABITACIÓN

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            if (habitacion == null)
            {
                return NotFound();
            }

            // TOTAL

            int dias = (fechaSalida - fechaEntrada).Days;
            if (dias <= 0)
            {
                dias = 1;
            }

            decimal total = dias * habitacion.Precio;

            // NUEVA RESERVA

            var nuevaReserva = new Reserva
            {
                UsuarioId = userId.Value,

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
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // =========================
        // CREATE ADMIN
        // =========================

        public IActionResult Create()
        {
            ViewData["HabitacionId"] = new SelectList(
                _context.Habitaciones.OrderBy(h => h.Numero),
                "Id",
                "Numero");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            var fechaEntrada = reserva.FechaEntrada.Date;

            if (fechaEntrada < DateTime.Today)
            {
                ViewBag.Error = "No puedes crear reservas pasadas.";

                ViewData["HabitacionId"] = new SelectList(
                    _context.Habitaciones.OrderBy(h => h.Numero),
                    "Id",
                    "Numero");

                return View(reserva);
            }

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            if (habitacion == null)
            {
                return NotFound();
            }

            int dias = (reserva.FechaSalida - reserva.FechaEntrada).Days;

            decimal total = dias * habitacion.Precio;

            reserva.Total = total;

            reserva.FechaReserva = DateTime.Now;

            _context.Reservas.Add(reserva);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT
        // =========================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            ViewData["HabitacionId"] = new SelectList(
                _context.Habitaciones,
                "Id",
                "Numero",
                reserva.HabitacionId);

            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(reserva);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(reserva);
        }

        // =========================
        // DELETE
        // =========================

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
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