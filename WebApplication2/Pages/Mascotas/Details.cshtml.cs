using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Mascotas;

public class DetailsModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DetailsModel(VeterinariaService svc) => _svc = svc;

    public Mascota? Mascota { get; set; }
    public List<Cita> Citas { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        Mascota = _svc.ObtenerMascotaPorId(id, resolverNavegacion: true);
        if (Mascota is null)
        {
            TempData["Error"] = "Mascota no encontrada.";
            return RedirectToPage("Index");
        }
        Citas = _svc.ObtenerCitas()
                    .Where(c => c.MascotaId == id)
                    .OrderByDescending(c => c.FechaHora)
                    .ToList();
        return Page();
    }
}
