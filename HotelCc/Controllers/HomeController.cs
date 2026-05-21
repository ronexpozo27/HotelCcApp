using HotelCc.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelCc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalHabitaciones = _context.Habitaciones.Count();
            ViewBag.TotalReservas = _context.Reservas.Count();
            ViewBag.TotalUsuarios = _context.Usuarios.Count();

            ViewBag.Ingresos = _context.Reservas.Sum(r => r.Total);

            var reservas = _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .OrderByDescending(r => r.Id)
                .Take(10)
                .ToList();

            ViewBag.Reservas = reservas;

            return View();
        }
    }
}