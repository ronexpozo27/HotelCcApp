
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelCc.Data;
using HotelCc.Models;
using HotelCc.Filters;

namespace HotelCc.Controllers
{
    [AuthorizeRole("Admin")]
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
            return View(await _context.Habitaciones.ToListAsync());
        }
    }
}
