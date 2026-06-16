using HotelCc.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelCc.Controllers
{
    public class PagosController : Controller
    {
        private readonly AppDbContext _context;

        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");

            var userId = HttpContext.Session.GetInt32("UserId");

            if (rol == "Admin")
            {
                var pagosAdmin = await _context.Pagos
                    .Include(p => p.Reserva)
                        .ThenInclude(r => r.Usuario)
                    .Include(p => p.Reserva)
                        .ThenInclude(r => r.Habitacion)
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();

                return View(pagosAdmin);
            }

            var pagosUsuario = await _context.Pagos
                .Include(p => p.Reserva)
                    .ThenInclude(r => r.Usuario)
                .Include(p => p.Reserva)
                    .ThenInclude(r => r.Habitacion)
                .Where(p => p.Reserva.UsuarioId == userId)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(pagosUsuario);
        }

        public async Task<IActionResult> VerTicket(int id)
        {
            var pago = await _context.Pagos
                .Include(p => p.Reserva)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pago == null)
                return NotFound();

            return RedirectToAction(
                "Ticket",
                "Reservas",
                new { id = pago.ReservaId });
        }
    }
}