using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Mascotas;

public class EditModel : PageModel
{
    private readonly VeterinariaService _svc;

    public EditModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Mascota? Mascota { get; set; }

    public SelectList PropietariosSelectList { get; set; } = default!;

    public IActionResult OnGet(int id)
    {
        Mascota = _svc.ObtenerMascotaPorId(id, false);
        if (Mascota is null)
        {
            TempData["Error"] = "Mascota no encontrada.";
            return RedirectToPage("Index");
        }
        CargarSelectLists();
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            CargarSelectLists();
            return Page();
        }
        if (Mascota is null) return RedirectToPage("Index");

        var ok = _svc.ActualizarMascota(Mascota);
        if (!ok)
        {
            TempData["Error"] = "No se pudo actualizar la mascota.";
            return RedirectToPage("Index");
        }

        TempData["Exito"] = $"Mascota '{Mascota.Nombre}' actualizada correctamente.";
        return RedirectToPage("Index");
    }

    private void CargarSelectLists()
    {
        var propietarios = _svc.ObtenerPropietarios();
        PropietariosSelectList = new SelectList(propietarios, "Id", "NombreCompleto");
    }
}
