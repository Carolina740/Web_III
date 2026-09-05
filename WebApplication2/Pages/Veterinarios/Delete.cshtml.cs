using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Veterinarios;

public class DeleteModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DeleteModel(VeterinariaService svc) => _svc = svc;

    public Veterinario? Veterinario   { get; set; }
    public bool         TieneCitas    { get; set; }
    public int          CantidadCitas { get; set; }

    public IActionResult OnGet(int id)
    {
        Veterinario = _svc.ObtenerVeterinarioPorId(id);
        if (Veterinario is null)
        {
            TempData["Error"] = "Veterinario no encontrado.";
            return RedirectToPage("Index");
        }

        CantidadCitas = _svc.ObtenerCitas(false).Count(c => c.VeterinarioId == id);
        TieneCitas    = CantidadCitas > 0;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        if (_svc.ObtenerCitas(false).Any(c => c.VeterinarioId == id))
        {
            TempData["Error"] = "No se puede eliminar un veterinario que tiene citas asignadas.";
            return RedirectToPage("Index");
        }

        var vet    = _svc.ObtenerVeterinarioPorId(id);
        var nombre = vet?.NombreCompleto ?? "Desconocido";

        _svc.EliminarVeterinario(id);
        TempData["Exito"] = $"Veterinario '{nombre}' eliminado correctamente.";
        return RedirectToPage("Index");
    }
}
