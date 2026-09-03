using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Propietarios;

public class DetailsModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DetailsModel(VeterinariaService svc) => _svc = svc;

    public Propietario? Propietario { get; set; }
    public List<Mascota> Mascotas   { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        Propietario = _svc.ObtenerPropietarioPorId(id);
        if (Propietario is null)
        {
            TempData["Error"] = "Propietario no encontrado.";
            return RedirectToPage("Index");
        }

        Mascotas = _svc.ObtenerMascotasPorPropietario(id);
        return Page();
    }
}
