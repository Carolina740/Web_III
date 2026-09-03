using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Mascotas;

public class DeleteModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DeleteModel(VeterinariaService svc) => _svc = svc;

    public Mascota? Mascota       { get; set; }
    public bool     TieneCitas    { get; set; }
    public int      CantidadCitas { get; set; }

    public IActionResult OnGet(int id)
    {
        Mascota = _svc.ObtenerMascotaPorId(id, resolverNavegacion: true);
        if (Mascota is null)
        {
            TempData["Error"] = "Mascota no encontrada.";
            return RedirectToPage("Index");
        }

        CantidadCitas = _svc.ObtenerCitas().Count(c => c.MascotaId == id);
        TieneCitas    = CantidadCitas > 0;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        if (_svc.TieneCitas(id))
        {
            TempData["Error"] = "No se puede eliminar una mascota que tiene citas registradas.";
            return RedirectToPage("Index");
        }

        var mascota = _svc.ObtenerMascotaPorId(id, false);
        var nombre  = mascota?.Nombre ?? "Desconocida";

        _svc.EliminarMascota(id);
        TempData["Exito"] = $"Mascota '{nombre}' eliminada correctamente.";
        return RedirectToPage("Index");
    }
}
