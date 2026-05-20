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
            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion == null)
                return NotFound();

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
        // POST RESERVAR USUARIO
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar(Reserva reserva)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var fechaEntrada = DateTime.SpecifyKind(reserva.FechaEntrada.Date, DateTimeKind.Unspecified);
            var fechaSalida = DateTime.SpecifyKind(reserva.FechaSalida.Date, DateTimeKind.Unspecified);

            if (fechaSalida <= fechaEntrada)
            {
                ViewBag.Error = "Fecha inválida";
                return View(reserva);
            }

            bool ocupada = await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == reserva.HabitacionId &&
                fechaEntrada < r.FechaSalida &&
                fechaSalida > r.FechaEntrada);

            if (ocupada)
            {
                ViewBag.Error = "Habitación ocupada";
                return View(reserva);
            }

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            int dias = (fechaSalida - fechaEntrada).Days;
            if (dias <= 0) dias = 1;

            decimal total = dias * habitacion!.Precio;

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

            return RedirectToAction("Index");
        }

        // =========================
        // CREATE ADMIN (USUARIO O EXTERNO)
        // =========================
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");

            ViewData["HabitacionId"] = new SelectList(
                _context.Habitaciones
                    .OrderBy(h => h.Numero),
                "Id",
                "Numero"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            if (habitacion == null)
                return NotFound();

            var fechaEntrada = reserva.FechaEntrada.Date;
            var fechaSalida = reserva.FechaSalida.Date;

            if (reserva.UsuarioId != null)
            {
                reserva.NombreHuespedExterno = null;
            }
            else
            {
                reserva.UsuarioId = null;
            }

            int dias = (fechaSalida - fechaEntrada).Days;
            if (dias <= 0) dias = 1;

            decimal total = dias * habitacion.Precio;

            var nuevaReserva = new Reserva
            {
                UsuarioId = reserva.UsuarioId,
                NombreHuespedExterno = reserva.NombreHuespedExterno,
                HabitacionId = reserva.HabitacionId,
                FechaEntrada = fechaEntrada,
                FechaSalida = fechaSalida,
                FechaReserva = DateTime.Now,
                Total = total
            };

            _context.Reservas.Add(nuevaReserva);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
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