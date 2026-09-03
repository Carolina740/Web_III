using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Propietarios;

public class EditModel : PageModel
{
    private readonly VeterinariaService _svc;

    public EditModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Propietario? Propietario { get; set; }

    public IActionResult OnGet(int id)
    {
        Propietario = _svc.ObtenerPropietarioPorId(id);
        if (Propietario is null)
        {
            TempData["Error"] = "Propietario no encontrado.";
            return RedirectToPage("Index");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        if (Propietario is null) return RedirectToPage("Index");

        var ok = _svc.ActualizarPropietario(Propietario);
        if (!ok)
        {
            TempData["Error"] = "No se pudo actualizar el propietario.";
            return RedirectToPage("Index");
        }

        TempData["Exito"] = $"Propietario '{Propietario.NombreCompleto}' actualizado correctamente.";
        return RedirectToPage("Index");
    }
}
