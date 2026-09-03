using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Veterinarios;

public class DetailsModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DetailsModel(VeterinariaService svc) => _svc = svc;

    public Veterinario? Veterinario      { get; set; }
    public List<Cita>   CitasPendientes  { get; set; } = new();
    public List<Cita>   TodasLasCitas    { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        Veterinario = _svc.ObtenerVeterinarioPorId(id);
        if (Veterinario is null)
        {
            TempData["Error"] = "Veterinario no encontrado.";
            return RedirectToPage("Index");
        }

        var citas = _svc.ObtenerCitas()
                        .Where(c => c.VeterinarioId == id)
                        .OrderBy(c => c.FechaHora)
                        .ToList();

        TodasLasCitas   = citas;
        CitasPendientes = citas.Where(c => c.Estado == EstadoCita.Pendiente).ToList();
        return Page();
    }
}
