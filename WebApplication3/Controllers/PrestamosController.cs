using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
        {
            _context = context;
        }

        private string? Rol => HttpContext.Session.GetString("UsuarioRol");
        private int? UsuarioId => HttpContext.Session.GetInt32("UsuarioId");

        // Admin y Bibliotecario: todos los préstamos
        public async Task<IActionResult> Index()
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            if (Rol == "Usuario") return RedirectToAction("MisPrestamos");

            var prestamos = await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();

            return View(prestamos);
        }

        // Solo para usuarios normales
        public async Task<IActionResult> MisPrestamos()
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");

            var prestamos = await _context.Prestamos
                .Include(p => p.Libro)
                .Where(p => p.UsuarioId == UsuarioId)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();

            return View(prestamos);
        }

        public async Task<IActionResult> Crear()
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            if (Rol == "Usuario") return Forbid();

            ViewBag.Usuarios = new SelectList(
                await _context.Usuarios.Where(u => u.Activo && u.Rol == "Usuario").OrderBy(u => u.Nombre).ToListAsync(),
                "Id", "Nombre");

            ViewBag.Libros = new SelectList(
                await _context.Libros.Where(l => l.CantidadDisponible > 0).OrderBy(l => l.Titulo).ToListAsync(),
                "Id", "Titulo");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Prestamo prestamo)
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            if (Rol == "Usuario") return Forbid();

            if (ModelState.IsValid)
            {
                prestamo.FechaPrestamo = DateTime.Now;
                prestamo.Estado = "Activo";

                // Descontar del stock
                var libro = await _context.Libros.FindAsync(prestamo.LibroId);
                if (libro != null && libro.CantidadDisponible > 0)
                {
                    libro.CantidadDisponible--;
                    _context.Update(libro);
                }

                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Préstamo registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Usuarios = new SelectList(
                await _context.Usuarios.Where(u => u.Activo && u.Rol == "Usuario").OrderBy(u => u.Nombre).ToListAsync(),
                "Id", "Nombre", prestamo.UsuarioId);

            ViewBag.Libros = new SelectList(
                await _context.Libros.Where(l => l.CantidadDisponible > 0).OrderBy(l => l.Titulo).ToListAsync(),
                "Id", "Titulo", prestamo.LibroId);

            return View(prestamo);
        }

        // Devolver libro
        public async Task<IActionResult> Devolver(int id)
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            if (Rol == "Usuario") return Forbid();

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null) return NotFound();
            return View(prestamo);
        }

        [HttpPost, ActionName("Devolver")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DevolverConfirmado(int id)
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            if (Rol == "Usuario") return Forbid();

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo != null)
            {
                prestamo.Estado = "Devuelto";
                prestamo.FechaDevolucion = DateTime.Now;

                // Devolver al stock
                if (prestamo.Libro != null)
                    prestamo.Libro.CantidadDisponible++;

                await _context.SaveChangesAsync();
                TempData["Exito"] = "Devolución registrada correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
