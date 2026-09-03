using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Propietarios;

public class DeleteModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DeleteModel(VeterinariaService svc) => _svc = svc;

    public Propietario?  Propietario     { get; set; }
    public bool          TieneMascotas   { get; set; }
    public int           CantidadMascotas { get; set; }

    public IActionResult OnGet(int id)
    {
        Propietario = _svc.ObtenerPropietarioPorId(id);
        if (Propietario is null)
        {
            TempData["Error"] = "Propietario no encontrado.";
            return RedirectToPage("Index");
        }

        CantidadMascotas = _svc.ObtenerMascotasPorPropietario(id).Count;
        TieneMascotas    = CantidadMascotas > 0;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        // Verificación de seguridad: no eliminar si tiene mascotas
        if (_svc.TieneMascotas(id))
        {
            TempData["Error"] = "No se puede eliminar un propietario que tiene mascotas registradas.";
            return RedirectToPage("Index");
        }

        var propietario = _svc.ObtenerPropietarioPorId(id);
        var nombre      = propietario?.NombreCompleto ?? "Desconocido";

        _svc.EliminarPropietario(id);
        TempData["Exito"] = $"Propietario '{nombre}' eliminado correctamente.";
        return RedirectToPage("Index");
    }
}
