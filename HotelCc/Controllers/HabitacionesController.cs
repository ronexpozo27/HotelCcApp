
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelCc.Data;
using HotelCc.Models;
using HotelCc.Filters;

namespace HotelCc.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly AppDbContext _context;

        public HabitacionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Habitaciones
        public async Task<IActionResult> Index()
        {
            var habitaciones = await _context.Habitaciones.ToListAsync();
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
    }
}
