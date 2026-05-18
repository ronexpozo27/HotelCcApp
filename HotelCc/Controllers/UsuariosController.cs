using HotelCc.Data;
using HotelCc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelCc.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        private bool EsAdmin()
        {
            return HttpContext.Session.GetString("Rol") == "Admin";
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            var usuarios = await _context.Usuarios
                .OrderBy(u => u.Rol)
                .ThenBy(u => u.Nombre)
                .ToListAsync();

            return View(usuarios);
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // CREATE
        public IActionResult Create()
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            if (id != usuario.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(usuario);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}