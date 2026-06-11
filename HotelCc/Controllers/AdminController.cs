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
            // =========================
            // KPIs
            // =========================
            ViewBag.TotalHabitaciones = await _context.Habitaciones.CountAsync();
            ViewBag.TotalReservas = await _context.Reservas.CountAsync();
            ViewBag.TotalUsuarios = await _context.Usuarios.CountAsync();

            // =========================
            // HABITACIONES DISPONIBLES (CORRECTO)
            // =========================
            var habitacionesOcupadas = await _context.Reservas
                .Select(r => r.HabitacionId)
                .Distinct()
                .ToListAsync();

            ViewBag.HabitacionesDisponibles = await _context.Habitaciones
                .CountAsync(h => !habitacionesOcupadas.Contains(h.Id));

            // =========================
            // RESERVAS RECIENTES
            // =========================
            ViewBag.ReservasRecientes = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .OrderByDescending(r => r.FechaReserva)
                .Take(5)
                .ToListAsync();

            // =========================
            // INGRESOS ESTIMADOS
            // =========================
            var reservas = await _context.Reservas
                .Include(r => r.Habitacion)
                .ToListAsync();

            decimal ingresos = reservas.Sum(r =>
                r.Habitacion.Precio *
                Math.Max(1, (r.FechaSalida - r.FechaEntrada).Days)
            );

            ViewBag.Ingresos = ingresos;

            return View();
        }
    }
}