using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Veterinarios;

public class EditModel : PageModel
{
    private readonly VeterinariaService _svc;

    public EditModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Veterinario? Veterinario { get; set; }

    public IActionResult OnGet(int id)
    {
        Veterinario = _svc.ObtenerVeterinarioPorId(id);
        if (Veterinario is null)
        {
            TempData["Error"] = "Veterinario no encontrado.";
            return RedirectToPage("Index");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        if (Veterinario is null) return RedirectToPage("Index");

        var ok = _svc.ActualizarVeterinario(Veterinario);
        if (!ok)
        {
            TempData["Error"] = "No se pudo actualizar el veterinario.";
            return RedirectToPage("Index");
        }

        TempData["Exito"] = $"Veterinario '{Veterinario.NombreCompleto}' actualizado correctamente.";
        return RedirectToPage("Index");
    }
}
