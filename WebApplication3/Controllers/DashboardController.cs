using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;

namespace WebApplication3.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BibliotecaContext _context;

        public DashboardController(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol == null) return RedirectToAction("Login", "Cuenta");

            var totalLibros = await _context.Libros.CountAsync();
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var prestamosActivos = await _context.Prestamos.CountAsync(p => p.Estado == "Activo");
            var prestamosVencidos = await _context.Prestamos.CountAsync(p => p.Estado == "Vencido");

            ViewBag.TotalLibros = totalLibros;
            ViewBag.TotalUsuarios = totalUsuarios;
            ViewBag.PrestamosActivos = prestamosActivos;
            ViewBag.PrestamosVencidos = prestamosVencidos;
            ViewBag.Rol = rol;

            // Para usuario normal, solo sus préstamos
            if (rol == "Usuario")
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                var misPrestamos = await _context.Prestamos
                    .Include(p => p.Libro)
                    .Where(p => p.UsuarioId == usuarioId)
                    .OrderByDescending(p => p.FechaPrestamo)
                    .Take(5)
                    .ToListAsync();
                ViewBag.MisPrestamos = misPrestamos;
            }

            return View();
        }
    }
}
