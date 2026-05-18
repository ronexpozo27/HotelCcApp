using HotelCc.Data;
using HotelCc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelCc.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly AppDbContext _context;

        public HabitacionesController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX
        // =========================
        public async Task<IActionResult> Index()
        {
            var habitaciones = await _context.Habitaciones
                .OrderBy(h => h.Numero)
                .ToListAsync();

            var reservas = await _context.Reservas.ToListAsync();

            var hoy = DateTime.Today;

            var model = habitaciones.Select(h => new HabitacionEstadoViewModel
            {
                Id = h.Id,
                Numero = h.Numero,
                Tipo = h.Tipo,
                Precio = h.Precio,

                Estado = reservas.Any(r =>
                    r.HabitacionId == h.Id &&
                    r.FechaEntrada <= hoy &&
                    r.FechaSalida >= hoy)
                    ? "Ocupada"
                    : "Disponible"

            }).ToList();

            return View(model);
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == id);

            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }

        // =========================
        // CREATE
        // =========================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habitacion);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(habitacion);
        }

        // =========================
        // EDIT
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Habitacion habitacion)
        {
            if (id != habitacion.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(habitacion);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(habitacion);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == id);

            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion != null)
            {
                _context.Habitaciones.Remove(habitacion);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}