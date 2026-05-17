using HotelCc.Data;
using HotelCc.Filters;
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

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");

            var userId = HttpContext.Session.GetInt32("UserId");

            if (rol == "Admin")
            {
                var reservasAdmin = await _context.Reservas
                    .Include(r => r.Usuario)
                    .Include(r => r.Habitacion)
                    .ToListAsync();

                return View(reservasAdmin);
            }

            var reservasUser = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .Where(r => r.UsuarioId == userId)
                .ToListAsync();

            return View(reservasUser);
        }

        // GET: Reservas/Details/5
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

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["HabitacionId"] = new SelectList(
                _context.Habitaciones
                    .OrderBy(h => h.Numero),
                "Id",
                "Numero");

            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            var reservas = await _context.Reservas.ToListAsync();

            bool ocupada = reservas.Any(r =>
                r.HabitacionId == reserva.HabitacionId &&
                reserva.FechaEntrada < r.FechaSalida &&
                reserva.FechaSalida > r.FechaEntrada);

            if (ocupada)
            {
                ViewBag.Error = "La habitación no está disponible en el rango de fechas seleccionado.";

                ViewData["HabitacionId"] = new SelectList(
                    _context.Habitaciones,
                    "Id",
                    "Numero",
                    reserva.HabitacionId);

                return View(reserva);
            }

            var habitacion = await _context.Habitaciones
    .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            int dias = (reserva.FechaSalida - reserva.FechaEntrada).Days;

            decimal total = dias * habitacion!.Precio;

            var nuevaReserva = new Reserva
            {
                UsuarioId = HttpContext.Session.GetInt32("UserId") ?? 0,

                HabitacionId = reserva.HabitacionId,

                FechaEntrada = DateTime.SpecifyKind(
                    reserva.FechaEntrada,
                    DateTimeKind.Utc),

                FechaSalida = DateTime.SpecifyKind(
                    reserva.FechaSalida,
                    DateTimeKind.Utc),

                FechaReserva = DateTime.UtcNow,

                Total = total
            };

            _context.Reservas.Add(nuevaReserva);

            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Reservas/Edit/5
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

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios,
                "Id",
                "Nombre",
                reserva.UsuarioId);

            return View(reserva);
        }

        // POST: Reservas/Edit/5
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
                try
                {
                    _context.Update(reserva);



                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(reserva);
        }

        // GET: Reservas/Delete/5
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

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva != null)
            {
                var habitacion = await _context.Habitaciones
                    .FindAsync(reserva.HabitacionId);

                if (habitacion != null)
                {
                }

                _context.Reservas.Remove(reserva);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}