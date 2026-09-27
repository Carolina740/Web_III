using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly BibliotecaContext _context;

        public UsuariosController(BibliotecaContext context)
        {
            _context = context;
        }

        private string? Rol => HttpContext.Session.GetString("UsuarioRol");

        private IActionResult? VerificarAdmin()
        {
            if (Rol == null) return RedirectToAction("Login", "Cuenta");
            if (Rol != "Administrador") return Forbid();
            return null;
        }

        public async Task<IActionResult> Index()
        {
            var check = VerificarAdmin();
            if (check != null) return check;

            var usuarios = await _context.Usuarios.OrderBy(u => u.Nombre).ToListAsync();
            return View(usuarios);
        }

        public IActionResult Crear()
        {
            var check = VerificarAdmin();
            if (check != null) return check;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            var check = VerificarAdmin();
            if (check != null) return check;

            if (ModelState.IsValid)
            {
                var existe = await _context.Usuarios.AnyAsync(u => u.Correo == usuario.Correo);
                if (existe)
                {
                    ModelState.AddModelError("Correo", "Ya existe un usuario con ese correo.");
                    return View(usuario);
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Usuario registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var check = VerificarAdmin();
            if (check != null) return check;

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Usuario usuario)
        {
            var check = VerificarAdmin();
            if (check != null) return check;
            if (id != usuario.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Usuario actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var check = VerificarAdmin();
            if (check != null) return check;

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var check = VerificarAdmin();
            if (check != null) return check;

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Usuario eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
