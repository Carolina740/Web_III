using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        private string? Rol => HttpContext.Session.GetString("UsuarioRol");

        private IActionResult RedirigirSiNoAutenticado()
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            return null!;
        }

        // Admin y Bibliotecario
        public async Task<IActionResult> Index()
        {
            var r = RedirigirSiNoAutenticado();
            if (r != null) return r;

            if (Rol == "Usuario") return RedirectToAction("Catalogo");

            var libros = await _context.Libros.OrderBy(l => l.Titulo).ToListAsync();
            return View(libros);
        }

        // Vista solo lectura para usuarios normales
        public async Task<IActionResult> Catalogo()
        {
            var r = RedirigirSiNoAutenticado();
            if (r != null) return r;

            var libros = await _context.Libros
                .Where(l => l.CantidadDisponible > 0)
                .OrderBy(l => l.Titulo)
                .ToListAsync();
            return View(libros);
        }

        public IActionResult Crear()
        {
            var r = RedirigirSiNoAutenticado();
            if (r != null) return r;
            if (Rol == "Usuario") return Forbid();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Libro libro)
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            if (Rol == "Usuario") return Forbid();

            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Libro registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var r = RedirigirSiNoAutenticado();
            if (r != null) return r;
            if (Rol == "Usuario") return Forbid();

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Libro libro)
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            if (id != libro.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(libro);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Libro actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var r = RedirigirSiNoAutenticado();
            if (r != null) return r;
            if (Rol != "Administrador") return Forbid();

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            if (Rol != "Administrador") return Forbid();

            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Libro eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
