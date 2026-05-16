using HotelCc.Data;
using HotelCc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelCc.Controllers
{
    [AuthorizeRole("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalHabitaciones =
                _context.Habitaciones.Count();

            ViewBag.TotalReservas =
                _context.Reservas.Count();

            ViewBag.TotalUsuarios =
                _context.Usuarios.Count();

            ViewBag.HabitacionesDisponibles =
                _context.Habitaciones.Count(h => h.Disponible);

            // Reservas recientes
            ViewBag.ReservasRecientes = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .OrderByDescending(r => r.FechaReserva)
                .Take(5)
                .ToListAsync();

            // Ingresos estimados
            var reservas = await _context.Reservas
                .Include(r => r.Habitacion)
                .ToListAsync();

            decimal ingresos = reservas.Sum(r =>
                r.Habitacion.Precio *
                (decimal)(r.FechaSalida - r.FechaEntrada).Days);

            ViewBag.Ingresos = ingresos;

            return View();
        }
    }
}
